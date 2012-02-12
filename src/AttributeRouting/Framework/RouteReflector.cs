using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
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
            var controllerRouteSpecs = GenerateRouteSpecifications(_configuration.PromotedControllerTypes);
            foreach (var spec in controllerRouteSpecs)
                yield return spec;

            if (!_configuration.Assemblies.Any())
                yield break;

            var scannedControllerTypes = _configuration.Assemblies.SelectMany(a => a.GetControllerTypes()).ToList();
            var remainingControllerTypes = scannedControllerTypes.Except(_configuration.PromotedControllerTypes);

            var remainingRouteSpecs = GenerateRouteSpecifications(remainingControllerTypes);

            foreach (var spec in remainingRouteSpecs)
                yield return spec;
        }

        private IEnumerable<RouteSpecification> GenerateRouteSpecifications(IEnumerable<Type> controllerTypes)
        {
            var controllerCount = 0;

            return (from controllerType in controllerTypes
                    let controllerIndex = controllerCount++
                    let convention = controllerType.GetCustomAttribute<RouteConventionAttribute>(false)
                    let routeAreaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true)
                    let routePrefixAttribute = controllerType.GetCustomAttribute<RoutePrefixAttribute>(true)
                    from actionMethod in controllerType.GetActionMethods()
                    from routeAttribute in GetRouteAttributes(actionMethod, convention)
                    orderby controllerIndex, routeAttribute.Precedence
                    // precedence is within a controller
                    let routeName = routeAttribute.RouteName
                    select new RouteSpecification
                    {
                        AreaName = routeAreaAttribute.SafeGet(a => a.AreaName),
                        AreaUrl = routeAreaAttribute.SafeGet(a => a.AreaUrl ?? a.AreaName),
                        AreaUrlTranslationKey = routeAreaAttribute.SafeGet(a => a.TranslationKey),
                        RoutePrefixUrl = GetRoutePrefix(routePrefixAttribute, actionMethod, convention),
                        RoutePrefixUrlTranslationKey = routePrefixAttribute.SafeGet(a => a.TranslationKey),
                        ControllerType = controllerType,
                        ControllerName = controllerType.GetControllerName(),
                        ActionName = actionMethod.Name,
                        ActionParameters = actionMethod.GetParameters(),
                        RouteUrl = routeAttribute.RouteUrl,
                        RouteUrlTranslationKey = routeAttribute.TranslationKey,
                        HttpMethods = routeAttribute.HttpMethods,
                        DefaultAttributes = GetDefaultAttributes(actionMethod, routeName, convention),
                        ConstraintAttributes = GetConstraintAttributes(actionMethod, routeName, convention),
                        RouteName = routeName,
                        IsAbsoluteUrl = routeAttribute.IsAbsoluteUrl
                    }).ToList();
        }

        private static IEnumerable<RouteAttribute> GetRouteAttributes(MethodInfo actionMethod,
                                                                      RouteConventionAttribute convention)
        {
            var attributes = new List<RouteAttribute>();

            // Add convention-based attributes
            if (convention != null)
                attributes.AddRange(convention.GetRouteAttributes(actionMethod));

            // Add explicitly-defined attributes
            attributes.AddRange(actionMethod.GetCustomAttributes<RouteAttribute>(false));

            return attributes.OrderBy(a => a.Order);
        }

        private static string GetRoutePrefix(RoutePrefixAttribute routePrefixAttribute, MethodInfo actionMethod, RouteConventionAttribute convention)
        {
            // Return an explicitly defined route prefix, if defined
            if (routePrefixAttribute != null)
                return routePrefixAttribute.Url;

            // Otherwise, if this is a convention-based controller, get the convention-based prefix
            if (convention != null)
                return convention.GetDefaultRoutePrefix(actionMethod);

            return null;
        }

        private static ICollection<RouteDefaultAttribute> GetDefaultAttributes(MethodInfo actionMethod, string routeName,
                                                                               RouteConventionAttribute convention)
        {
            var defaultAttributes = new List<RouteDefaultAttribute>();

            // Yield explicitly defined default attributes first
            defaultAttributes.AddRange(
                from defaultAttribute in actionMethod.GetCustomAttributes<RouteDefaultAttribute>(false)
                where !defaultAttribute.ForRouteNamed.HasValue() ||
                      defaultAttribute.ForRouteNamed == routeName
                select defaultAttribute);

            // Yield convention-based defaults next
            if (convention != null)
                defaultAttributes.AddRange(convention.GetRouteDefaultAttributes(actionMethod));

            return defaultAttributes.ToList();
        }

        private static ICollection<RouteConstraintAttribute> GetConstraintAttributes(MethodInfo actionMethod,
                                                                                     string routeName,
                                                                                     RouteConventionAttribute convention)
        {
            var constraintAttributes = new List<RouteConstraintAttribute>();

            // Yield explicitly defined constraint attributes first
            constraintAttributes.AddRange(
                from constraintAttribute in actionMethod.GetCustomAttributes<RouteConstraintAttribute>(false)
                where !constraintAttribute.ForRouteNamed.HasValue() ||
                      constraintAttribute.ForRouteNamed == routeName
                select constraintAttribute);

            // Yield convention-based constraints next
            if (convention != null)
                constraintAttributes.AddRange(convention.GetRouteConstraintAtributes(actionMethod));

            return constraintAttributes.ToList();
        }
    }
}