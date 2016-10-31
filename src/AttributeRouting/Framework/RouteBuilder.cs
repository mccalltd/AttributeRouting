﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Creates <see cref="IAttributeRoute"/> objects according to the 
    /// options set in implementations of <see cref="ConfigurationBase"/>.
    /// </summary>    
    public class RouteBuilder
    {
        private static readonly Regex ConstraintParamsRegex = new Regex(@"^.*\(.*\)$");
        
        private static readonly Regex DetokenizeUrlParamContentsRegex 
            = new Regex(@"(" +
                        @":.+" + // inline constraints
                        @"|=.+" + // or inline defaults
                        @"|\?" + // or inline optionals
                        @")$" // at end of string
                        );

        private static readonly Regex DetokenizeUrlRegex =
            new Regex(@"\?(?=\})" + // trailing question mark (used to specify optional param)
                      @"|\(.*?\)(?=\})" + // or stuff inside parens (used to specify inline regex route constraint)
                      @"|\:(.*?)(\(.*?\))?((?=\})|(?=\?\}))" + // or new inline constraint syntax
                      @"|(?<=\{.*)=.*?(?=\})" // or equals and value (used to specify inline parameter default value)
                );

        private readonly ConfigurationBase _configuration;
        private readonly IParameterFactory _parameterFactory;
        private readonly IAttributeRouteFactory _routeFactory;
        private readonly IRouteConstraintFactory _routeConstraintFactory;

        public RouteBuilder(ConfigurationBase configuration)
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
            // Get the route specifications that describe the routes we'll build.
            var reflector = new AttributeReflector(_configuration);
            var routeSpecs = reflector.BuildRouteSpecifications().ToList();

            // Update the config object with context.
            _configuration.MappedSubdomains = (from s in routeSpecs
                                               where s.Subdomain.HasValue()
                                               select s.Subdomain).Distinct().ToList();

            return routeSpecs.SelectMany(BuildRoutes);
        }

        private void ApplyDefaultRouteConstraint(IEnumerable<string> urlParameterNames, KeyValuePair<string, object> constraint, IDictionary<string, object> targetConstraints, bool inQueryString)
        {
            foreach (var name in urlParameterNames)
            {
                if (!Regex.IsMatch(name, constraint.Key) || targetConstraints.ContainsKey(name))
                {
                    continue;
                }

                var finalConstraint = constraint.Value;
                if (inQueryString)
                {
                    finalConstraint = _routeConstraintFactory.CreateQueryStringRouteConstraint(finalConstraint);
                }
                targetConstraints.Add(name, finalConstraint);
            }            
        }

        private string BuildTokenizedUrl(string routeUrl, string routePrefixUrl, string areaUrl, RouteSpecification routeSpec)
        {
            var delimitedUrl = routeUrl + "/";

            // Prepend prefix if available
            if (routePrefixUrl.HasValue() && !routeSpec.IgnoreRoutePrefix)
            {
                var delimitedRoutePrefix = routePrefixUrl + "/";
                if (!delimitedUrl.StartsWith(delimitedRoutePrefix))
                {
                    delimitedUrl = delimitedRoutePrefix + delimitedUrl;
                }
            }

            // Prepend area url if available
            if (areaUrl.HasValue() && !routeSpec.IgnoreAreaUrl)
            {
                var delimitedAreaUrl = areaUrl + "/";
                if (!delimitedUrl.StartsWith(delimitedAreaUrl))
                {
                    delimitedUrl = delimitedAreaUrl + delimitedUrl;
                }
            }

            return delimitedUrl.Trim('/');
        }

        private IEnumerable<IAttributeRoute> BuildRoutes(RouteSpecification routeSpec)
        {
            // Get info needed to construct IAttributeRoute via factory method.
            IDictionary<string, object> defaults;
            IDictionary<string, object> queryStringDefaults;
            CreateRouteDefaults(routeSpec, out defaults, out queryStringDefaults);
            IDictionary<string, object> constraints;
            IDictionary<string, object> queryStringConstraints;
            CreateRouteConstraints(routeSpec, out constraints, out queryStringConstraints);
            var dataTokens = CreateRouteDataTokens(routeSpec);
            var url = CreateRouteUrl(defaults, routeSpec);

            // Build the route.
            var routes = _routeFactory.CreateAttributeRoutes(url, defaults, constraints, dataTokens);

            // Extend the factory generated route:
            foreach (var route in routes)
            {
                var routeName = CreateRouteName(routeSpec);
                if (routeName.HasValue())
                {
                    route.RouteName = routeName;
                    route.DataTokens.Add("routeName", routeName);
                }

                route.QueryStringConstraints = queryStringConstraints;
                route.QueryStringDefaults = queryStringDefaults;
                route.Translations = CreateRouteTranslations(routeSpec);
                route.Subdomain = routeSpec.Subdomain;
                route.UseLowercaseRoute = routeSpec.UseLowercaseRoute;
                route.PreserveCaseForUrlParameters = routeSpec.PreserveCaseForUrlParameters;
                route.AppendTrailingSlash = routeSpec.AppendTrailingSlash;

                // Yield the default route first
                yield return route;

                // Then yield the translations
                if (route.Translations != null)
                {
                    foreach (var translation in route.Translations)
                    {
                        // Backreference the default route.
                        translation.SourceLanguageRoute = route;

                        yield return translation;
                    }
                }
            }
        }

        private void CreateRouteConstraints(RouteSpecification routeSpec, out IDictionary<string, object> constraints, out IDictionary<string, object> queryStringConstraints)
        {
            // Going to return individual collections for:
            // - path routes constraints (which will go into the generated route's Constraints prop),
            // - and query string route constraints (which will not work perfectly with the MS bits, and need special treatment by IAttributeRoute impls).
            constraints = new Dictionary<string, object>();
            queryStringConstraints = new Dictionary<string, object>();

            // Default constraints
            if (routeSpec.HttpMethods.Any())
            {
                constraints.Add("inboundHttpMethod", _routeConstraintFactory.CreateInboundHttpMethodConstraint(routeSpec.HttpMethods));
            }

            // Work from a complete, tokenized url; ie: support constraints in area urls, route prefix urls, and route urls.
            var tokenizedUrl = BuildTokenizedUrl(routeSpec.RouteUrl, routeSpec.RoutePrefixUrl, routeSpec.AreaUrl, routeSpec);
            var urlParameters = GetUrlParameterContents(tokenizedUrl).ToList();

            // Need to keep track of which are path and query params. 
            var pathOnlyUrl = RemoveQueryString(tokenizedUrl);
            var pathOnlyUrlParameters = GetUrlParameterContents(pathOnlyUrl);
            var queryStringParameters = urlParameters.Except(pathOnlyUrlParameters).ToList();

            // Inline constraints
            foreach (var parameter in urlParameters)
            {
                // Keep track of whether this param is optional or in the querystring, 
                // because we wrap the final constraint if so.
                var parameterIsOptional = parameter.EndsWith("?");
                var parameterIsInQueryString = queryStringParameters.Contains(parameter);

                // If this is a path parameter and doesn't have a constraint, then skip it.
                if (!parameterIsInQueryString && !parameter.Contains(":"))
                {
                    continue;
                }

                // Strip off everything related to defaults and break into sections.
                var cleanParameter = parameter.TrimEnd('?').Split('=').FirstOrDefault();
                var sections = cleanParameter.SplitAndTrim(":");
                
                // Do not override default constraints
                var parameterName = sections.First();
                if (constraints.ContainsKey(parameterName))
                {
                    continue;
                }

                // Add constraints for each inline definition
                var inlineConstraints = new List<object>();
                var constraintDefinitions = sections.Skip(1);
                foreach (var definition in constraintDefinitions)
                {
                    string constraintName;
                    object constraint;

                    if (ConstraintParamsRegex.IsMatch(definition))
                    {
                        // Constraint of the form "firstName:string(50)"
                        var indexOfOpenParen = definition.IndexOf('(');
                        constraintName = definition.Substring(0, indexOfOpenParen);
                        
                        // Parse constraint params. 
                        // NOTE: Splitting on commas only applies to non-regex constraints.
                        var constraintParamsRaw = definition.Substring(indexOfOpenParen + 1, definition.Length - indexOfOpenParen - 2);
                        var constraintParams = constraintName.ValueEquals("regex")
                                                   ? new[] { constraintParamsRaw }
                                                   : constraintParamsRaw.SplitAndTrim(",");

                        constraint = _routeConstraintFactory.CreateInlineRouteConstraint(constraintName, constraintParams);
                    }
                    else
                    {
                        // Constraint of the form "id:int"
                        constraintName = definition;
                        constraint = _routeConstraintFactory.CreateInlineRouteConstraint(constraintName);
                    }

                    if (constraint == null)
                    {
                        throw new AttributeRoutingException(
                            "Could not find an available inline constraint for \"{0}\".".FormatWith(constraintName));
                    }

                    inlineConstraints.Add(constraint);
                }

                // Wrap constraints in the following priority: 
                object finalConstraint;

                // 1. If more than one constraint, wrap in a compound constraint.
                if (inlineConstraints.Count > 1)
                {
                    finalConstraint = _routeConstraintFactory.CreateCompoundRouteConstraint(inlineConstraints.ToArray());
                }
                else
                {
                    finalConstraint = inlineConstraints.FirstOrDefault();
                }

                // 2. If the constraint is in the querystring, wrap in a query string constraint.
                if (parameterIsInQueryString)
                {
                    finalConstraint = _routeConstraintFactory.CreateQueryStringRouteConstraint(finalConstraint);
                }

                // 3. If the constraint is optional, wrap in an optional constraint.
                if (parameterIsOptional)
                {
                    finalConstraint = _routeConstraintFactory.CreateOptionalRouteConstraint(finalConstraint);
                }

                // Add the constraints to the appropriate collection.
                if (parameterIsInQueryString)
                {
                    queryStringConstraints.Add(parameterName, finalConstraint);
                }
                else
                {
                    constraints.Add(parameterName, finalConstraint);
                }
            } // ... go to next parameter

            // Globally configured constraints - have to treat path params differently than query params.
            var detokenizedUrl = DetokenizeUrl(tokenizedUrl);
            string path, query;
            detokenizedUrl.GetPathAndQuery(out path, out query);
            var urlPathParameterNames = GetUrlParameterContents(path).ToArray();
            var urlQueryParameterNames = GetUrlParameterContents(query).ToArray();

            foreach (var defaultConstraint in _configuration.DefaultRouteConstraints)
            {
                ApplyDefaultRouteConstraint(urlPathParameterNames, defaultConstraint, constraints, false);
                ApplyDefaultRouteConstraint(urlQueryParameterNames, defaultConstraint, queryStringConstraints, true);
            }
        }

        private IDictionary<string, object> CreateRouteDataTokens(RouteSpecification routeSpec)
        {
            var dataTokens = new Dictionary<string, object>
            {
                { "namespaces", new[] { routeSpec.ControllerType.Namespace } },
                { "actionMethod", routeSpec.ActionMethod },
                { "defaultSubdomain", _configuration.DefaultSubdomain}
            };

            if (routeSpec.HttpMethods.Any())
            {
                dataTokens.Add("httpMethods", routeSpec.HttpMethods);
            }

            if (routeSpec.AreaName.HasValue())
            {
                dataTokens.Add("area", routeSpec.AreaName);
                dataTokens.Add("UseNamespaceFallback", false);
            }

            return dataTokens;
        }

        private void CreateRouteDefaults(RouteSpecification routeSpec, out IDictionary<string, object> defaults, out IDictionary<string, object> queryStringDefaults)
        {
            // Going to return individual collections for:
            // - path routes defaults (which will go into the generated route's Defaults prop),
            // - and query string route defaults (which will not work perfectly with the MS bits, and need special treatment by IAttributeRoute impls).
            defaults = new Dictionary<string, object>
            {
                { "controller", routeSpec.ControllerName },
                { "action", routeSpec.ActionName }
            };
            queryStringDefaults = new Dictionary<string, object>();

            // Work from a complete, tokenized url; ie: support constraints in area urls, route prefix urls, and route urls.
            var tokenizedUrl = BuildTokenizedUrl(routeSpec.RouteUrl, routeSpec.RoutePrefixUrl, routeSpec.AreaUrl, routeSpec);
            var urlParameters = GetUrlParameterContents(tokenizedUrl).ToList();

            // Need to keep track of which are path and query params. 
            var pathOnlyUrl = RemoveQueryString(tokenizedUrl);
            var pathOnlyUrlParameters = GetUrlParameterContents(pathOnlyUrl).ToList();
            var queryStringParameters = urlParameters.Except(pathOnlyUrlParameters).ToList();

            // Inspect the url path for optional parameters and default values.
            foreach (var parameter in urlParameters)
            {
                // Does this param have a default value or is it optional?
                var isOptional = parameter.EndsWith("?");
                var indexOfEquals = parameter.IndexOf('=');
                if (!isOptional && indexOfEquals == -1)
                {
                    continue;
                }

                // Keep track if this is a querystring param
                var parameterIsInQueryString = queryStringParameters.Contains(parameter);

                // Strip off inline constraints, defaults, and optional tokens.
                var parameterName = DetokenizeUrlParamContentsRegex.Replace(parameter, "");

                // Do not override default defaults.
                if (defaults.ContainsKey(parameterName))
                {
                    continue;
                }

                object defaultValue;
                if (isOptional)
                {
                    defaultValue = _parameterFactory.Optional();
                }
                else // It has a default value.
                {
                    defaultValue = parameter.Substring(indexOfEquals + 1, parameter.Length - indexOfEquals - 1);
                }
                
                if (parameterIsInQueryString)
                {
                    queryStringDefaults.Add(parameterName, defaultValue);
                }
                else
                {
                    defaults.Add(parameterName, defaultValue);                    
                }
            }
        }

        private string CreateRouteName(RouteSpecification routeSpec)
        {
            if (routeSpec.RouteName.HasValue())
            {
                return routeSpec.RouteName;
            }

            return _configuration.AutoGenerateRouteNames ? _configuration.RouteNameBuilder(routeSpec) : null;
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
                {
                    continue;
                }

                //*********************************************
                // Otherwise, build a translated route

                // REVIEW: Could probably forgo processing defaults, constraints, and data tokens for translated routes.

                IDictionary<string, object> defaults;
                IDictionary<string, object> queryStringDefaults;
                CreateRouteDefaults(routeSpec, out defaults, out queryStringDefaults);
                IDictionary<string, object> constraints;
                IDictionary<string, object> queryStringConstraints;
                CreateRouteConstraints(routeSpec, out constraints, out queryStringConstraints);
                var dataTokens = CreateRouteDataTokens(routeSpec);
                var routeUrl = CreateRouteUrl(translatedRouteUrl ?? routeSpec.RouteUrl,
                                              translatedRoutePrefix ?? routeSpec.RoutePrefixUrl,
                                              translatedAreaUrl ?? routeSpec.AreaUrl,
                                              defaults,
                                              routeSpec);

                var translatedRoutes = _routeFactory.CreateAttributeRoutes(routeUrl, defaults, constraints, dataTokens);

                foreach (var translatedRoute in translatedRoutes)
                {
                    var routeName = CreateRouteName(routeSpec);
                    if (routeName != null)
                    {
                        translatedRoute.RouteName = routeName;
                        translatedRoute.DataTokens.Add("routeName", routeName);
                    }

                    translatedRoute.QueryStringConstraints = queryStringConstraints;
                    translatedRoute.QueryStringDefaults = queryStringDefaults;
                    translatedRoute.CultureName = cultureName;
                    translatedRoute.DataTokens.Add("cultureName", cultureName);

                    yield return translatedRoute;
                }
            }
        }

        private string CreateRouteUrl(IDictionary<string, object> defaults, RouteSpecification routeSpec)
        {
            return CreateRouteUrl(routeSpec.RouteUrl,
                                  routeSpec.RoutePrefixUrl,
                                  routeSpec.AreaUrl,
                                  defaults,
                                  routeSpec);
        }

        private string CreateRouteUrl(string routeUrl, string routePrefix, string areaUrl, IDictionary<string, object> defaults, RouteSpecification routeSpec)
        {
            var tokenizedUrl = BuildTokenizedUrl(routeUrl, routePrefix, areaUrl, routeSpec);
            var tokenizedPath = RemoveQueryString(tokenizedUrl);
            var detokenizedPath = DetokenizeUrl(tokenizedPath);
            var urlParameterNames = GetUrlParameterContents(detokenizedPath).ToList();

            var urlBuilder = new StringBuilder(detokenizedPath);

            // Replace {controller} URL param with default value.
            if (urlParameterNames.Any(n => n.ValueEquals("controller")))
            {
                urlBuilder.Replace("{controller}", (string)defaults["controller"]);
            }

            // Replace {action} URL param with default value.
            if (urlParameterNames.Any(n => n.ValueEquals("action")))
            {
                urlBuilder.Replace("{action}", (string)defaults["action"]);
            }

            // Explicitly defined area routes are not valid
            if (urlParameterNames.Any(n => n.ValueEquals("area")))
            {
                throw new AttributeRoutingException(
                    "{area} url parameters are not allowed. Specify the area name by using the RouteAreaAttribute.");
            }

            // If we are lowercasing routes, then lowercase everything but the route params
            var lower = routeSpec.UseLowercaseRoute.GetValueOrDefault(_configuration.UseLowercaseRoutes);
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
                        {
                            i++;
                        }
                    }
                }
            }

            return urlBuilder.ToString().Trim('/');
        }

        private static string DetokenizeUrl(string url)
        {
            return DetokenizeUrlRegex.Replace(url, "");
        }

        private static IEnumerable<string> GetUrlParameterContents(string url)
        {
            if (!url.HasValue())
            {
                yield break;
            }

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

        private static string RemoveQueryString(string url)
        {
            // Must honor ? in regex expressions and as used to specify optional params,
            // So run through the url chars and fast forward when inside a url param (eg: {...})
            for (int i = 0, length = url.Length; i < length; i++)
            {
                var c = url[i];
                if (c == '?')
                {
                    return url.Substring(0, i);
                }
                
                // Fast-forward past url param contents
                if (c == '{')
                {
                    while (i < length && url[i] != '}') //check length prior to accessing the character
                    {
                        i++;
                    }
                }
            }

            return url;
        }
    }
}