﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Creates <see cref="IAttributeRoute"/> objects according to the 
    /// options set in implementations of <see cref="AttributeRoutingConfigurationBase"/>.
    /// </summary>    
    public class RouteBuilder
    {
        private readonly AttributeRoutingConfigurationBase _configuration;
        private readonly IAttributeRouteFactory _routeFactory;
        private readonly IRouteConstraintFactory _routeConstraintFactory;
        private readonly IParameterFactory _parameterFactory;

        public RouteBuilder(AttributeRoutingConfigurationBase configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
            _routeFactory = configuration.AttributeRouteFactory;
            _routeConstraintFactory = configuration.RouteConstraintFactory;
            _parameterFactory = configuration.ParameterFactory;
        }

        /// <summary>
        /// Yields all the routes to register in the route table.
        /// </summary>
        public IEnumerable<IAttributeRoute> BuildAllRoutes()
        {
            var routeReflector = new RouteReflector(_configuration);
            var routeSpecs = routeReflector.BuildRouteSpecifications().ToList();

            var mappedSubdomains = (from s in routeSpecs
                                    where s.Subdomain.HasValue()
                                    select s.Subdomain).Distinct().ToList();

            foreach (var routeSpec in routeSpecs)
            {
                foreach (var route in Build(routeSpec))
                {
                    route.MappedSubdomains = mappedSubdomains;
                    yield return route;
                }
            }
        }

        private IEnumerable<IAttributeRoute> Build(RouteSpecification routeSpec)
        {
            var route = _routeFactory.CreateAttributeRoute(CreateRouteUrl(routeSpec),
                                                           CreateRouteDefaults(routeSpec),
                                                           CreateRouteConstraints(routeSpec),
                                                           CreateRouteDataTokens(routeSpec));

            var routeName = CreateRouteName(routeSpec);
            if (routeName.HasValue())
            {
                route.RouteName = routeName;
                route.DataTokens.Add("routeName", routeName);
            }

            route.Translations = CreateRouteTranslations(routeSpec);
            route.Subdomain = routeSpec.Subdomain;
            route.UseLowercaseRoute = routeSpec.UseLowercaseRoute;
            route.PreserveCaseForUrlParameters = routeSpec.PreserveCaseForUrlParameters;
            route.AppendTrailingSlash = routeSpec.AppendTrailingSlash;

            // Yield the default route first
            yield return route;

            // Then yield the translations
            if (route.Translations == null)
                yield break;

            foreach (var translation in route.Translations)
            {
                // Backreference the default route.
                translation.DefaultRouteContainer = route;

                yield return translation;
            }
        }

        private string CreateRouteName(RouteSpecification routeSpec)
        {
            if (routeSpec.RouteName.HasValue())
                return routeSpec.RouteName;

            return _configuration.AutoGenerateRouteNames ? _configuration.RouteNameBuilder(routeSpec) : null;
        }

        private string CreateRouteUrl(RouteSpecification routeSpec)
        {
            return CreateRouteUrl(routeSpec.RouteUrl,
                                  routeSpec.RoutePrefixUrl,
                                  routeSpec.AreaUrl,
                                  routeSpec.IsAbsoluteUrl,
                                  routeSpec.UseLowercaseRoute);
        }

        private string CreateRouteUrl(string routeUrl, string routePrefix, string areaUrl, bool isAbsoluteUrl, bool? useLowercaseRoute)
        {
            var tokenizedUrl = BuildTokenizedUrl(routeUrl, routePrefix, areaUrl, isAbsoluteUrl);
            var detokenizedUrl = DetokenizeUrl(tokenizedUrl);

            var urlParameterNames = GetUrlParameterContents(detokenizedUrl).ToList();

            // {controller} and {action} tokens are not valid
            if (urlParameterNames.Any(n => n.ValueEquals("controller")))
                throw new AttributeRoutingException("{controller} is not a valid url parameter.");
            if (urlParameterNames.Any(n => n.ValueEquals("action")))
                throw new AttributeRoutingException("{action} is not a valid url parameter.");

            // Explicitly defined area routes are not valid
            if (urlParameterNames.Any(n => n.ValueEquals("area")))
                throw new AttributeRoutingException(
                    "{area} url parameters are not allowed. Specify the area name by using the RouteAreaAttribute.");

            var urlBuilder = new StringBuilder(detokenizedUrl);

            // If we are lowercasing routes, then lowercase everything but the route params
            var lower = useLowercaseRoute.HasValue ? useLowercaseRoute.Value : _configuration.UseLowercaseRoutes;
            if (lower)
            {
                for (var i = 0; i < urlBuilder.Length; i++)
                {
                    var c = urlBuilder[i];
                    if (Char.IsUpper(c))
                    {
                        urlBuilder[i] = Char.ToLower(c);
                    }
                    else if (c == '{')
                    {
                        while (urlBuilder[i] != '}' && i < urlBuilder.Length)
                            i++;
                    }
                }
            }

            return urlBuilder.ToString().Trim('/');
        }

        private IDictionary<string, object> CreateRouteDefaults(RouteSpecification routeSpec)
        {
            var defaults = new Dictionary<string, object>
            {
                { "controller", routeSpec.ControllerName },
                { "action", routeSpec.ActionName }
            };

            var urlParameters = GetUrlParameterContents(routeSpec.RouteUrl).ToList();

            // Inspect the url for optional parameters, specified with a trailing ?
            foreach (var parameter in urlParameters.Where(p => p.EndsWith("?")))
            {
                var parameterName = parameter.TrimEnd('?');
                
                if (parameterName.Contains(':'))
                    parameterName = parameterName.Substring(0, parameterName.IndexOf(':'));

                if (defaults.ContainsKey(parameterName))
                    continue;

                defaults.Add(parameterName, _parameterFactory.Optional());
            }

            // Inline defaults
            foreach (var parameter in urlParameters.Where(p => p.Contains('=')))
            {
                var indexOfEquals = parameter.IndexOf('=');
                var parameterName = parameter.Substring(0, indexOfEquals);

                if (parameterName.Contains(':'))
                    parameterName = parameterName.Substring(0, parameterName.IndexOf(':'));

                if (defaults.ContainsKey(parameterName))
                    continue;

                var defaultValue = parameter.Substring(indexOfEquals + 1, parameter.Length - indexOfEquals - 1);
                defaults.Add(parameterName, defaultValue);
            }

            return defaults;
        }

        private IDictionary<string, object> CreateRouteConstraints(RouteSpecification routeSpec)
        {
            var constraints = new Dictionary<string, object>();

            // Default constraints
            if (routeSpec.HttpMethods.Any())
                constraints.Add("httpMethod", _routeConstraintFactory.CreateRestfulHttpMethodConstraint(routeSpec.HttpMethods));

            // Work from a complete, tokenized url; ie: support constraints in area urls, route prefix urls, and route urls.
            var tokenizedUrl = BuildTokenizedUrl(routeSpec.RouteUrl, routeSpec.RoutePrefixUrl, routeSpec.AreaUrl, routeSpec.IsAbsoluteUrl);
            var urlParameters = GetUrlParameterContents(tokenizedUrl).ToList();

            // Inline constraints
            var constraintFactory = _configuration.RouteConstraintFactory;
            foreach (var parameter in urlParameters.Where(p => p.Contains(":")))
            {
                // Keep track of whether this param is optional, 
                // because we wrap the final constraint if so.
                var parameterIsOptional = parameter.EndsWith("?");

                // Strip off everything related to defaults.
                var cleanParameter = parameter.TrimEnd('?').Split('=').FirstOrDefault();

                var sections = cleanParameter.SplitAndTrim(":");
                var parameterName = sections.First();

                // Do not override default or legacy inline constraints
                if (constraints.ContainsKey(parameterName))
                    continue;

                // Add constraints for each inline definition
                var inlineConstraints = new List<object>();
                var constraintDefinitions = sections.Skip(1);
                foreach (var definition in constraintDefinitions)
                {
                    string constraintName;
                    object constraint;

                    if (Regex.IsMatch(definition, @"^.*\(.*\)$"))
                    {
                        // Constraint of the form "firstName:string(50)"
                        var indexOfOpenParen = definition.IndexOf('(');
                        constraintName = definition.Substring(0, indexOfOpenParen);
                        var constraintParams = definition.Substring(indexOfOpenParen + 1, definition.Length - indexOfOpenParen - 2).SplitAndTrim(",");
                        constraint = constraintFactory.CreateInlineRouteConstraint(constraintName, constraintParams);
                    }
                    else
                    {
                        // Constraint of the form "id:int"
                        constraintName = definition;
                        constraint = constraintFactory.CreateInlineRouteConstraint(constraintName);
                    }

                    if (constraint == null)
                        throw new AttributeRoutingException(
                            "Could not find an available inline constraint for \"{0}\".".FormatWith(constraintName));

                    inlineConstraints.Add(constraint);
                }

                // Apply the constraint to the parameter. 
                // Wrap multiple constraints in a compound constraint wrapper.
                // Wrap constraints for optional params in an optional constraint wrapper.
                var finalConstraint = (inlineConstraints.Count == 1)
                                          ? inlineConstraints.Single()
                                          : constraintFactory.CreateCompoundRouteConstraint(inlineConstraints.ToArray());

                if (parameterIsOptional)
                    constraints.Add(parameterName, constraintFactory.CreateOptionalRouteConstraint(finalConstraint));
                else
                    constraints.Add(parameterName, finalConstraint);
            }

            // Globally configured constraints
            var detokenizedPrefixedUrl = DetokenizeUrl(tokenizedUrl);
            var urlParameterNames = GetUrlParameterContents(detokenizedPrefixedUrl).ToList();
            foreach (var defaultConstraint in _configuration.DefaultRouteConstraints)
            {
                var pattern = defaultConstraint.Key;

                foreach (var urlParameterName in urlParameterNames.Where(n => Regex.IsMatch(n, pattern)))
                {
                    if (constraints.ContainsKey(urlParameterName))
                        continue;

                    constraints.Add(urlParameterName, defaultConstraint.Value);
                }
            }

            return constraints;
        }

        private string BuildTokenizedUrl(string routeUrl, string routePrefixUrl, string areaUrl, bool isAbsoluteUrl)
        {
            var delimitedUrl = routeUrl + "/";
            
            if (!isAbsoluteUrl)
            {
                // Prepend prefix if available
                if (routePrefixUrl.HasValue())
                {
                    var delimitedRoutePrefix = routePrefixUrl + "/";
                    if (!delimitedUrl.StartsWith(delimitedRoutePrefix))
                        delimitedUrl = delimitedRoutePrefix + delimitedUrl;
                }

                // Prepend area url if available
                if (areaUrl.HasValue())
                {
                    var delimitedAreaUrl = areaUrl + "/";
                    if (!delimitedUrl.StartsWith(delimitedAreaUrl))
                        delimitedUrl = delimitedAreaUrl + delimitedUrl;
                }
            }

            return delimitedUrl.Trim('/');
        }

        private IDictionary<string, object> CreateRouteDataTokens(RouteSpecification routeSpec)
        {
            var dataTokens = new Dictionary<string, object>
            {
                { "namespaces", new[] { routeSpec.ControllerType.Namespace } }
            };

            if (routeSpec.AreaName.HasValue())
            {
                dataTokens.Add("area", routeSpec.AreaName);
                dataTokens.Add("UseNamespaceFallback", false);
            }

            return dataTokens;
        }

        private static string DetokenizeUrl(string url)
        {
            var patterns = new List<string>
            {
                @"(?<=\{)\?",                           // leading question mark (used to specify optional param)
                @"\?(?=\})",                            // trailing question mark (used to specify optional param)
                @"\(.*?\)(?=\})",                       // stuff inside parens (used to specify inline regex route constraint)
                @"\:(.*?)(\(.*?\))?((?=\})|(?=\?\}))",  // new inline constraint syntax
                @"(?<=\{.*)=.*?(?=\})",                 // equals and value (used to specify inline parameter default value)
            };

            return Regex.Replace(url, String.Join("|", patterns), "");
        }

        private IEnumerable<IAttributeRoute> CreateRouteTranslations(RouteSpecification routeSpec)
        {
            // If no translation provider, then get out of here.
            if (!_configuration.TranslationProviders.Any())
                yield break;

            // Merge all the culture names from the various providers.
            var cultureNames = _configuration.GetTranslationProviderCultureNames();

            // Built the route translations, 
            // choosing the first available translated route component from among the providers
            foreach (var cultureName in cultureNames)
            {
                string translatedRouteUrl = null,
                       translatedRoutePrefix = null,
                       translatedAreaUrl = null;

                foreach (var provider in _configuration.TranslationProviders)
                {
                    translatedRouteUrl = translatedRouteUrl ?? provider.TranslateRouteUrl(cultureName, routeSpec);
                    translatedRoutePrefix = translatedRoutePrefix ?? provider.TranslateRoutePrefix(cultureName, routeSpec);
                    translatedAreaUrl = translatedAreaUrl ?? provider.TranslateAreaUrl(cultureName, routeSpec);
                }

                // If nothing is translated, then bail.
                if (translatedRouteUrl == null && translatedRoutePrefix == null && translatedAreaUrl == null)
                    continue;

                //*********************************************
                // Otherwise, build a translated route

                var routeUrl = CreateRouteUrl(translatedRouteUrl ?? routeSpec.RouteUrl,
                                              translatedRoutePrefix ?? routeSpec.RoutePrefixUrl,
                                              translatedAreaUrl ?? routeSpec.AreaUrl,
                                              routeSpec.IsAbsoluteUrl,
                                              routeSpec.UseLowercaseRoute);

                var translatedRoute = _routeFactory.CreateAttributeRoute(routeUrl,
                                                                         CreateRouteDefaults(routeSpec),
                                                                         CreateRouteConstraints(routeSpec),
                                                                         CreateRouteDataTokens(routeSpec));

                var routeName = CreateRouteName(routeSpec);
                if (routeName != null)
                {
                    translatedRoute.RouteName = routeName;
                    translatedRoute.DataTokens.Add("routeName", routeName);
                }

                translatedRoute.CultureName = cultureName;
                translatedRoute.DataTokens.Add("cultureName", cultureName);

                yield return translatedRoute;
            }
        }

        private static IEnumerable<string> GetUrlParameterContents(string url)
        {
            if (!url.HasValue())
                yield break;

            var urlSegments = url.SplitAndTrim(new[] { "/" });
            foreach (var urlSegment in urlSegments)
            {
                // Find an open curly in the segment, and if none, then move on to the next.
                var iOpenCurly = urlSegment.IndexOf('{');
                if (iOpenCurly == -1) continue;

                var i = iOpenCurly + 1;
                while (i < urlSegment.Length)
                {
                    if (urlSegment[i] == '}')
                    {
                        // If we find the closing curly, then yield the contents of the url param.
                        yield return urlSegment.Substring(iOpenCurly + 1, i - iOpenCurly - 1);

                        // Fast-forward to the next open curly brace.
                        iOpenCurly = urlSegment.IndexOf('{', i);
                        if (iOpenCurly == -1) break;
                        i = iOpenCurly;
                    }
                    else if (urlSegment[i] == '{')
                    {
                        // If we find an inner open curly (due to inner regex patterns), 
                        // then fast-forward beyond it.
                        i = urlSegment.IndexOf('}', i);                        
                    }

                    i++;
                }
            }
        }
    }
}