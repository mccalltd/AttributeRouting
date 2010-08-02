using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public IEnumerable<AttributeRouteSpecification> GenerateRouteSpecifications()
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

        private IEnumerable<AttributeRouteSpecification> GetRouteSpecifications(IEnumerable<Type> controllerTypes)
        {
            return (from controllerType in controllerTypes
                    from actionMethod in controllerType.GetActionMethods()
                    from routeAttribute in actionMethod.GetRouteAttributes()
                    let routeName = routeAttribute.RouteName
                    select new AttributeRouteSpecification
                    {
                        AreaName = GetAreaName(actionMethod),
                        RoutePrefix = GetRoutePrefix(actionMethod),
                        ControllerType = controllerType,
                        ControllerName = controllerType.GetControllerName(),
                        ActionName = actionMethod.Name,
                        ActionParameters = actionMethod.GetParameters(),
                        Url = routeAttribute.Url,
                        HttpMethod = routeAttribute.HttpMethod,
                        DefaultAttributes = GetDefaultAttributes(actionMethod, routeName),
                        ConstraintAttributes = GetConstraintAttributes(actionMethod, routeName),
                        RouteName = routeName
                    }).ToList();
        }

        private string GetAreaName(MethodInfo actionMethod)
        {
            var routeAreaAttribute = actionMethod.GetRouteAreaAttribute();
            if (routeAreaAttribute != null)
                return routeAreaAttribute.AreaName;

            return "";
        }

        private string GetRoutePrefix(MethodInfo actionMethod)
        {
            var routePrefixAttribute = actionMethod.GetRoutePrefixAttribute();
            if (routePrefixAttribute != null)
                return routePrefixAttribute.Url;

            return "";
        }

        private IEnumerable<RouteDefaultAttribute> GetDefaultAttributes(MethodInfo actionMethod, string routeName)
        {
            return from defaultAttribute in actionMethod.GetCustomAttributes<RouteDefaultAttribute>(false)
                   where !defaultAttribute.ForRouteNamed.HasValue() ||
                         defaultAttribute.ForRouteNamed == routeName
                   select defaultAttribute;
        }

        private IEnumerable<RouteConstraintAttribute> GetConstraintAttributes(MethodInfo actionMethod, string routeName)
        {
            return from constraintAttribute in actionMethod.GetCustomAttributes<RouteConstraintAttribute>(false)
                   where !constraintAttribute.ForRouteNamed.HasValue() ||
                         constraintAttribute.ForRouteNamed == routeName
                   select constraintAttribute;
        }
    }
}
