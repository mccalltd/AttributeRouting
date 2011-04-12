using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Extensions;

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

        public AttributeRoute Build(RouteSpecification routeSpec)
        {
            return new AttributeRoute(CreateRouteName(routeSpec),
                                      CreateRouteUrl(routeSpec),
                                      CreateRouteDefaults(routeSpec),
                                      CreateRouteConstraints(routeSpec),
                                      CreateRouteDataTokens(routeSpec),
                                      _configuration.UseLowercaseRoutes);
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
            var detokenizedUrl = DetokenizeUrl(routeSpec.Url);
            var urlParameterNames = GetUrlParameterNames(detokenizedUrl);

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
            if (!routeSpec.IsAbsoluteUrl)
            {
                if (routeSpec.RoutePrefix.HasValue() && !routeSpec.Url.StartsWith(routeSpec.RoutePrefix))
                    urlBuilder.Insert(0, routeSpec.RoutePrefix + "/");

                if (routeSpec.AreaUrl.HasValue() && !routeSpec.Url.StartsWith(routeSpec.AreaUrl))
                    urlBuilder.Insert(0, routeSpec.AreaUrl + "/");
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

            foreach (var defaultAttribute in routeSpec.DefaultAttributes.Where(d => !defaults.ContainsKey(d.Key)))
                defaults.Add(defaultAttribute.Key, defaultAttribute.Value);

            // Inspect the url for optional parameters, specified with a leading ?
            var optionalParameterDefaults =
                from parameter in GetUrlParameterContents(routeSpec.Url)
                where parameter.StartsWith("?")
                let parameterName = parameter.TrimStart('?')
                select new RouteDefaultAttribute(parameterName, UrlParameter.Optional);

            foreach (var defautAttribute in optionalParameterDefaults.Where(d => !defaults.ContainsKey(d.Key)))
                defaults.Add(defautAttribute.Key, defautAttribute.Value);

            return defaults;
        }

        private RouteValueDictionary CreateRouteConstraints(RouteSpecification routeSpec)
        {
            var constraints = new RouteValueDictionary();

            // Default constraints
            constraints.Add("httpMethod", new RestfulHttpMethodConstraint(routeSpec.HttpMethods));

            // Attribute-based constraints
            foreach (var constraintAttribute in routeSpec.ConstraintAttributes.Where(c => !constraints.ContainsKey(c.Key)))
                constraints.Add(constraintAttribute.Key, constraintAttribute.Constraint);

            var detokenizedUrl = DetokenizeUrl(CreateRouteUrl(routeSpec));
            var urlParameterNames = GetUrlParameterNames(detokenizedUrl);

            // Convention-based constraints
            foreach (var defaultConstraint in _configuration.DefaultRouteConstraints)
            {
                var pattern = defaultConstraint.Key;
                var matchedUrlParameterNames = urlParameterNames.Where(n => Regex.IsMatch(n, pattern));
                foreach (var urlParameterName in matchedUrlParameterNames.Where(n => !constraints.ContainsKey(n)))
                    constraints.Add(urlParameterName, defaultConstraint.Value);
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
            return Regex.Replace(url, @"\{\?", "{");
        }

        private static IEnumerable<string> GetUrlParameterNames(string url)
        {
            return (from match in Regex.Matches(url, @"(?<={)\w*(?=})").Cast<Match>()
                    select match.Captures[0].ToString()).ToList();
        }

        private static IEnumerable<string> GetUrlParameterContents(string url)
        {
            return (from match in Regex.Matches(url, @"(?<={).*?(?=})").Cast<Match>()
                    select match.Captures[0].ToString()).ToList();
        }
    }
}