using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace AttributeRouting
{
    public class RouteReflector
    {
        private readonly AttributeRoutingConfiguration _configuration;

        public RouteReflector(AttributeRoutingConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        public IEnumerable<RouteSpecification> GenerateRouteSpecifications()
        {
            var controllerRouteSpecs = GetRouteSpecifications(_configuration.PromotedControllerTypes);
            foreach (var spec in controllerRouteSpecs)
                yield return spec;

            if (_configuration.AddScannedRoutes)
            {
                var scannedControllerTypes = _configuration.Assemblies.SelectMany(a => a.GetControllerTypes()).ToList();
                var remainingControllerTypes = scannedControllerTypes.Except(_configuration.PromotedControllerTypes);

                var remainingRouteSpecs = GetRouteSpecifications(remainingControllerTypes);

                foreach (var spec in remainingRouteSpecs)
                    yield return spec;
            }
        }

        private IEnumerable<RouteSpecification> GetRouteSpecifications(IEnumerable<Type> controllerTypes)
        {
            return (from controllerType in controllerTypes
                    from actionMethod in controllerType.GetActionMethods()
                    from routeAttribute in GetRouteAttributes(actionMethod)
                    orderby routeAttribute.Precedence, routeAttribute.Order
                    let routeName = routeAttribute.RouteName
                    select new RouteSpecification
                    {
                        AreaName = GetAreaName(actionMethod),
                        AreaUrl = GetAreaUrl(actionMethod),
                        RoutePrefix = GetRoutePrefix(actionMethod),
                        ControllerType = controllerType,
                        ControllerName = controllerType.GetControllerName(),
                        ActionName = actionMethod.Name,
                        ActionParameters = actionMethod.GetParameters(),
                        Url = routeAttribute.Url,
                        HttpMethod = routeAttribute.HttpMethod,
                        DefaultAttributes = GetDefaultAttributes(actionMethod, routeName),
                        ConstraintAttributes = GetConstraintAttributes(actionMethod, routeName),
                        RouteName = routeName,
                        IsAbsoluteUrl = routeAttribute.IsAbsoluteUrl
                    }).ToList();
        }

        private static IEnumerable<RouteAttribute> GetRouteAttributes(MethodInfo actionMethod)
        {
            // Yield convention-based attributes
            var conventionalAttributes =
                from attribute in actionMethod.DeclaringType.GetCustomAttributes<RestfulRouteConventionAttribute>(false)
                from routeAttribute in attribute.GetRouteAttributes(actionMethod)
                select routeAttribute;

            foreach (var conventionalAttribute in conventionalAttributes)
                yield return conventionalAttribute;

            // Yield explicitly-defined attributes
            var explicitAttributes = actionMethod.GetCustomAttributes<RouteAttribute>(false);
            foreach (var explicitAttribute in explicitAttributes)
                yield return explicitAttribute;
        }

        private static string GetAreaName(MethodInfo actionMethod)
        {
            var routeAreaAttribute = actionMethod.GetRouteAreaAttribute();
            if (routeAreaAttribute != null)
                return routeAreaAttribute.AreaName;

            return "";
        }

        private static string GetAreaUrl(MethodInfo actionMethod)
        {
            var routeAreaAttribute = actionMethod.GetRouteAreaAttribute();
            if (routeAreaAttribute != null)
                return routeAreaAttribute.AreaUrl;

            return "";
        }

        private static string GetRoutePrefix(MethodInfo actionMethod)
        {
            // Return an explicitly defined route prefix, if defined
            var routePrefixAttribute = actionMethod.GetRoutePrefixAttribute();
            if (routePrefixAttribute != null)
                return routePrefixAttribute.Url;

            // Otherwise, this is a convention-based controller, get the convention-based prefix
            var conventionAttribute = actionMethod.DeclaringType.GetCustomAttribute<RestfulRouteConventionAttribute>(false);
            if (conventionAttribute != null)
                return conventionAttribute.GetDefaultRoutePrefix(actionMethod);

            return "";
        }

        private static ICollection<RouteDefaultAttribute> GetDefaultAttributes(MethodInfo actionMethod, string routeName)
        {
            return (from defaultAttribute in actionMethod.GetCustomAttributes<RouteDefaultAttribute>(false)
                    where !defaultAttribute.ForRouteNamed.HasValue() ||
                          defaultAttribute.ForRouteNamed == routeName
                    select defaultAttribute).ToList();
        }

        private static ICollection<RouteConstraintAttribute> GetConstraintAttributes(MethodInfo actionMethod, string routeName)
        {
            return (from constraintAttribute in actionMethod.GetCustomAttributes<RouteConstraintAttribute>(false)
                    where !constraintAttribute.ForRouteNamed.HasValue() ||
                          constraintAttribute.ForRouteNamed == routeName
                    select constraintAttribute).ToList();
        }
    }
}
