using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{

    /// <summary>
    /// A static factory that eases creation of the RouteBuilder
    /// </summary>
    public static class RouteBuilderFactory {

        /// <summary>
        /// Create a new RouteBuilder with the given types
        /// </summary>
        /// <typeparam name="TRoute"></typeparam>
        /// <typeparam name="TRouteParameter"></typeparam>
        /// <typeparam name="TRequestContext"></typeparam>
        /// <typeparam name="TRouteData"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static RouteBuilder<TRoute, TRouteParameter, TRequestContext, TRouteData> Create<TRoute, TRouteParameter, TRequestContext, TRouteData>(
            AttributeRoutingConfiguration<TRoute, TRouteParameter, TRequestContext, TRouteData> configuration) {
            return new RouteBuilder<TRoute, TRouteParameter, TRequestContext, TRouteData>(configuration);
        }
    }

    /// <summary>
    /// Class that actually creates all the routes from attributes and AR configuration. Relies on RouteReflector to inspect types
    /// </summary>
    /// <typeparam name="TRoute"></typeparam>
    /// <typeparam name="TRouteParameter"></typeparam>
    /// <typeparam name="TRequestContext"></typeparam>
    /// <typeparam name="TRouteData"></typeparam>
    public class RouteBuilder<TRoute, TRouteParameter, TRequestContext, TRouteData>
    {

        private readonly AttributeRoutingConfiguration<TRoute, TRouteParameter, TRequestContext, TRouteData> _configuration;
        private readonly IAttributeRouteFactory _routeFactory;
        private readonly IConstraintFactory _constraintFactory;
        private readonly IParameterFactory<TRouteParameter> _parameterFactory;

        internal RouteBuilder(AttributeRoutingConfiguration<TRoute, TRouteParameter, TRequestContext, TRouteData> configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
            _routeFactory = configuration.AttributeFactory;
            _constraintFactory = configuration.ConstraintFactory;
            _parameterFactory = configuration.ParameterFactory;
        }

        public IEnumerable<IAttributeRoute> BuildAllRoutes()
        {
            var routeReflector = RouteReflectorFactory.Create(_configuration);
            var routeSpecs = routeReflector.GenerateRouteSpecifications().ToList();
            var mappedSubdomains = routeSpecs.Where(s => s.Subdomain.HasValue()).Select(s => s.Subdomain).Distinct().ToList();

            foreach (var routeSpec in routeSpecs)
            {
                foreach (var route in Build(routeSpec))
                {
                    route.MappedSubdomains = mappedSubdomains;
                    yield return route;                    
                }
            }
        }

        private IEnumerable<IAttributeRoute> Build(RouteSpecification routeSpec) {
            var route = _routeFactory.CreateAttributeRoute(CreateRouteUrl(routeSpec),
                                                           CreateRouteDefaults(routeSpec),
                                                           CreateRouteConstraints(routeSpec),
                                                           CreateRouteDataTokens(routeSpec));

            route.RouteName = CreateRouteName(routeSpec);
            route.Translations = CreateRouteTranslations(routeSpec);
            route.Subdomain = routeSpec.Subdomain;
            

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

            if (_configuration.AutoGenerateRouteNames)
            {
                var area = (routeSpec.AreaName.HasValue()) ? routeSpec.AreaName + "_" : null;
                return "{0}{1}_{2}".FormatWith(area, routeSpec.ControllerName, routeSpec.ActionName);
            }

            return null;
        }

        private string CreateRouteUrl(RouteSpecification routeSpec)
        {
            return CreateRouteUrl(routeSpec.RouteUrl, routeSpec.RoutePrefixUrl, routeSpec.AreaUrl, routeSpec.IsAbsoluteUrl);
        }

        private string CreateRouteUrl(string routeUrl, string routePrefix, string areaUrl, bool isAbsoluteUrl)
        {
            var detokenizedUrl = DetokenizeUrl(routeUrl);
            
            var urlParameterNames = GetUrlParameterContents(detokenizedUrl);
        
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

            // If this is not an absolute url, prefix with a route prefix or area name
            if (!isAbsoluteUrl)
            {
                var delimitedRouteUrl = routeUrl + "/";
  
                if (routePrefix.HasValue())
                {
                    var delimitedRoutePrefix = routePrefix + "/";
                    if (!delimitedRouteUrl.StartsWith(delimitedRoutePrefix))
                        urlBuilder.Insert(0, delimitedRoutePrefix);                    
                }

                if (areaUrl.HasValue())
                {
                    var delimitedAreaUrl = areaUrl + "/";
                    if (!delimitedRouteUrl.StartsWith(delimitedAreaUrl))
                        urlBuilder.Insert(0, delimitedAreaUrl);                    
                }
            }

            // If we are lowercasing routes, then lowercase everything but the route params
            if (_configuration.UseLowercaseRoutes)
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

            var urlParameters = GetUrlParameterContents(routeSpec.RouteUrl);

            // Inspect the url for optional parameters, specified with a leading or trailing (or both) ?
            foreach (var parameter in urlParameters.Where(p => Regex.IsMatch(p, @"^\?|\?$")))
            {
                var parameterName = parameter.Trim('?');

                if (defaults.ContainsKey(parameterName))
                    continue;

                defaults.Add(parameterName, _parameterFactory.Optional());
            }

            // Inline defaults
            foreach (var parameter in urlParameters.Where(p => Regex.IsMatch(p, @"^.*=.*$")))
            {
                var indexOfEquals = parameter.IndexOf('=');
                var parameterName = parameter.Substring(0, indexOfEquals);

                if (defaults.ContainsKey(parameterName))
                    continue;

                var defaultValue = parameter.Substring(indexOfEquals + 1, parameter.Length - indexOfEquals - 1);
                defaults.Add(parameterName, defaultValue);
            }

            // Attribute-based defaults
            foreach (var defaultAttribute in routeSpec.DefaultAttributes)
            {
                if (defaults.ContainsKey(defaultAttribute.Key))
                    continue;

                defaults.Add(defaultAttribute.Key, defaultAttribute.Value);
            }

            return defaults;
        }

        private IDictionary<string, object> CreateRouteConstraints(RouteSpecification routeSpec)
        {
            var constraints = new Dictionary<string, object>();

            // Default constraints
            if (routeSpec.HttpMethods.Any())
                constraints.Add("httpMethod", _constraintFactory.CreateRestfulHttpMethodConstraint(routeSpec.HttpMethods));

            // Inline constraints
            foreach (var parameter in GetUrlParameterContents(routeSpec.RouteUrl).Where(p => Regex.IsMatch(p, @"^.*\(.*\)$")))
            {
                var indexOfOpenParen = parameter.IndexOf('(');
                var parameterName = parameter.Substring(0, indexOfOpenParen);

                if (constraints.ContainsKey(parameterName))
                    continue;

                var regexPattern = parameter.Substring(indexOfOpenParen + 1, parameter.Length - indexOfOpenParen - 2);
                constraints.Add(parameterName, _constraintFactory.CreateRegexRouteConstraint(regexPattern));
            }

            // Attribute-based constraints
            foreach (var constraintAttribute in routeSpec.ConstraintAttributes)
            {
                if (constraints.ContainsKey(constraintAttribute.Key))
                    continue;

                constraints.Add(constraintAttribute.Key, constraintAttribute.Constraint);
            }

            var detokenizedUrl = DetokenizeUrl(CreateRouteUrl(routeSpec));
            var urlParameterNames = GetUrlParameterContents(detokenizedUrl);

            // Convention-based constraints
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

        private IDictionary<string, object> CreateRouteDataTokens(RouteSpecification routeSpec)
        {
            var dataTokens = new Dictionary<string, object>()
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
                @"(?<=\{)\?", // leading question mark (used to specify optional param)
                @"\?(?=\})", // trailing question mark (used to specify optional param)
                @"\(.*?\)(?=\})", // stuff inside parens (used to specify inline regex route constraint)
                @"=.*?(?=\})", // equals and value (used to specify inline parameter default value)
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

                if (translatedRouteUrl == null && translatedRoutePrefix == null && translatedAreaUrl == null)
                    continue;

                var translatedRoute =
                    _routeFactory.CreateAttributeRoute(CreateRouteUrl(translatedRouteUrl ?? routeSpec.RouteUrl,
                                                                      translatedRoutePrefix ?? routeSpec.RoutePrefixUrl,
                                                                      translatedAreaUrl ?? routeSpec.AreaUrl,
                                                                      routeSpec.IsAbsoluteUrl),
                                                       CreateRouteDefaults(routeSpec),
                                                       CreateRouteConstraints(routeSpec),
                                                       CreateRouteDataTokens(routeSpec));

                translatedRoute.CultureName = cultureName;

                translatedRoute.DataTokens.Add("cultureName", cultureName);

                yield return translatedRoute;
            }
        }

        private static List<string> GetUrlParameterContents(string url)
        {
            if (!url.HasValue())
                return new List<string>();

            return (from urlPart in url.SplitAndTrim(new[] { "/" })
                    from match in Regex.Matches(urlPart, @"(?<={).*(?=})").Cast<Match>()
                    select match.Captures[0].ToString()).ToList();
        }
    }
}