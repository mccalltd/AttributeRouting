using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace AttributeRouting
{
    public class AttributeRouteBuilder
    {
        private readonly AttributeRoutingConfiguration _configuration;

        public AttributeRouteBuilder(AttributeRoutingConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        public AttributeRoute Build(AttributeRouteSpecification routeSpec)
        {
            return new AttributeRoute(routeSpec.RouteName,
                                      CreateRouteUrl(routeSpec),
                                      CreateRouteDefaults(routeSpec),
                                      CreateRouteConstraints(routeSpec),
                                      CreateRouteDataTokens(routeSpec),
                                      _configuration.UseLowercaseRoutes);
        }

        private string CreateRouteUrl(AttributeRouteSpecification routeSpec)
        {
            var urlParameterNames = GetUrlParameterNames(routeSpec.Url);
            
            // {controller} and {action} tokens are not valid
            if (urlParameterNames.Any(n => n.ValueEquals("controller")))
                throw new AttributeRoutingException("{controller} is not a valid url parameter.");
            if (urlParameterNames.Any(n => n.ValueEquals("action")))
                throw new AttributeRoutingException("{action} is not a valid url parameter.");

            // Explicitly defined area routes are not valid
            if (urlParameterNames.Any(n => n.ValueEquals("area")))
                    throw new AttributeRoutingException(
                        "{area} url parameters are not allowed. Specify the area name by using the RouteAreaAttribute.");

            var urlBuilder = new StringBuilder(routeSpec.Url);

            if (routeSpec.RoutePrefix.HasValue())
                urlBuilder.Insert(0, routeSpec.RoutePrefix + "/");
            
            if (routeSpec.AreaName.HasValue())
                urlBuilder.Insert(0, routeSpec.AreaName + "/");

            return urlBuilder.ToString();
        }

        private RouteValueDictionary CreateRouteDefaults(AttributeRouteSpecification routeSpec)
        {
            var defaults = new RouteValueDictionary
            {
                { "controller", routeSpec.ControllerName },
                { "action", routeSpec.ActionName }
            };

            foreach (var defaultAttribute in routeSpec.DefaultAttributes.Where(d => !defaults.ContainsKey(d.Key)))
                defaults.Add(defaultAttribute.Key, defaultAttribute.Value);

            return defaults;
        }

        private RouteValueDictionary CreateRouteConstraints(AttributeRouteSpecification routeSpec)
        {
            var constraints = new RouteValueDictionary();

            // Default constraints
            constraints.Add("httpMethod", new RestfulHttpMethodConstraint(routeSpec.HttpMethod));

            // Attribute-based constraints
            foreach (var constraintAttribute in routeSpec.ConstraintAttributes.Where(c => !constraints.ContainsKey(c.Key)))
                constraints.Add(constraintAttribute.Key, constraintAttribute.Constraint);

            // Automatic constraints on primitive types
            if (_configuration.ConstrainPrimitiveRouteParameters)
            {
                var parametersToConstrain = from urlParameterName in GetUrlParameterNames(routeSpec.Url)
                                            where !constraints.ContainsKey(urlParameterName)
                                            from parameter in routeSpec.ActionParameters
                                            where parameter.Name == urlParameterName
                                            select parameter;

                foreach (var parameter in parametersToConstrain)
                {
                    var regex = GetRegexForType(parameter.ParameterType);
                    if (regex != null)
                        constraints.Add(parameter.Name, regex);
                }
            }

            return constraints;
        }

        private RouteValueDictionary CreateRouteDataTokens(AttributeRouteSpecification routeSpec)
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

        private static IEnumerable<string> GetUrlParameterNames(string url)
        {
            return (from match in Regex.Matches(url, @"(?<={)\w*(?=})").Cast<Match>()
                    select match.Captures[0].ToString()).ToList();
        }

        private static string GetRegexForType(Type type)
        {
            if (type == typeof(int))
                return @"[-+]?\b\d+\b";

            return null;
        }
    }
}
