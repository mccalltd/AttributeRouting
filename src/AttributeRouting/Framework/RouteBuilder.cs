using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    public class RouteBuilder
    {
        private readonly AttributeRoutingConfiguration _configuration;

        public RouteBuilder(AttributeRoutingConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        public IEnumerable<AttributeRoute> BuildAllRoutes()
        {
            var routeReflector = new RouteReflector(_configuration);
            var routeSpecs = routeReflector.GenerateRouteSpecifications();

            return routeSpecs.Select(Build);
        }

        private AttributeRoute Build(RouteSpecification routeSpec)
        {
            return new AttributeRoute(
                CreateRouteUrl(routeSpec),
                CreateRouteDefaults(routeSpec),
                CreateRouteConstraints(routeSpec),
                CreateRouteDataTokens(routeSpec),
                _configuration)
            {
                Name = CreateRouteName(routeSpec),
                Translations = CreateRouteTranslations(routeSpec)
            };
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

        private static string CreateRouteUrl(RouteSpecification routeSpec)
        {
            return CreateRouteUrl(routeSpec.RouteUrl, routeSpec.RoutePrefixUrl, routeSpec.AreaUrl, routeSpec.IsAbsoluteUrl);
        }

        private static string CreateRouteUrl(string routeUrl, string routePrefix, string areaUrl, bool isAbsoluteUrl)
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

            return urlBuilder.ToString().Trim('/');
        }

        private RouteValueDictionary CreateRouteDefaults(RouteSpecification routeSpec)
        {
            var defaults = new RouteValueDictionary
            {
                { "controller", routeSpec.ControllerName },
                { "action", routeSpec.ActionName }
            };

            var urlParameters = GetUrlParameterContents(routeSpec.RouteUrl);

            // Inspect the url for optional parameters, specified with a leading or trailing (or both) ?
            foreach (var parameter in urlParameters.Where(p => Regex.IsMatch(p, @"^\?|\?$")))
            {
                if (defaults.ContainsKey(parameter))
                    continue;

                var parameterName = parameter.Trim('?');
                defaults.Add(parameterName, UrlParameter.Optional);
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

        private RouteValueDictionary CreateRouteConstraints(RouteSpecification routeSpec)
        {
            var constraints = new RouteValueDictionary();

            // Default constraints
            if (routeSpec.HttpMethods.Any())
                constraints.Add("httpMethod", new RestfulHttpMethodConstraint(routeSpec.HttpMethods));

            // Inline constraints
            foreach (var parameter in GetUrlParameterContents(routeSpec.RouteUrl).Where(p => Regex.IsMatch(p, @"^.*\(.*\)$")))
            {
                var indexOfOpenParen = parameter.IndexOf('(');
                var parameterName = parameter.Substring(0, indexOfOpenParen);

                if (constraints.ContainsKey(parameterName))
                    continue;

                var regexPattern = parameter.Substring(indexOfOpenParen + 1, parameter.Length - indexOfOpenParen - 2);
                constraints.Add(parameterName, new RegexRouteConstraint(regexPattern));
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

        private RouteValueDictionary CreateRouteDataTokens(RouteSpecification routeSpec)
        {
            var dataTokens = new RouteValueDictionary
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

        private IEnumerable<AttributeRoute> CreateRouteTranslations(RouteSpecification routeSpec)
        {
            // If no translation provider, then get out of here.
            var translations = _configuration.TranslationProvider;
            if (translations == null)
                return null;

            var translatedRoutes = new List<AttributeRoute>();
            foreach (var cultureName in translations.CultureNames)
            {
                // Only yield a translated route if some part of the route is translated
                
                var routeUrl = translations.GetRouteUrl(cultureName, routeSpec);
                var routePrefix = translations.GetRoutePrefix(cultureName, routeSpec);
                var areaUrl = translations.GetAreaUrl(cultureName, routeSpec);

                if (routeUrl == null && routePrefix == null && areaUrl == null)
                    continue;

                var translatedRoute = new AttributeRoute(
                    CreateRouteUrl(routeUrl, routePrefix, areaUrl, routeSpec.IsAbsoluteUrl),
                    CreateRouteDefaults(routeSpec),
                    CreateRouteConstraints(routeSpec),
                    CreateRouteDataTokens(routeSpec),
                    _configuration)
                {
                    CultureName = cultureName
                };

                translatedRoutes.Add(translatedRoute);
            }

            return translatedRoutes;
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