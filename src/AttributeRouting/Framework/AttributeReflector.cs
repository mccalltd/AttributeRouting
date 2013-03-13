using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Creates <see cref="RouteSpecification"/> objects according to the 
    /// options set in implementations of <see cref="ConfigurationBase"/>.
    /// </summary>    
    public class AttributeReflector
    {
        private readonly ConfigurationBase _configuration;

        public AttributeReflector(ConfigurationBase configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        /// <summary>
        /// Yields all the information needed by <see cref="RouteBuilder"/> to build routes.
        /// </summary>
        public IEnumerable<RouteSpecification> BuildRouteSpecifications()
        {
            var controllerCount = 0; // needed to increment controller index
            var inheritActionsFromBaseController = _configuration.InheritActionsFromBaseController;

            var specs =
                // Each controller
                from controllerType in _configuration.OrderedControllerTypes
                let controllerIndex = controllerCount++
                let isAsyncController = controllerType.IsAsyncController()
                let routeAreaAttribute = GetRouteAreaAttribute(controllerType)
                // Each action
                from actionMethod in controllerType.GetActionMethods(inheritActionsFromBaseController)
                let actionName = GetActionName(actionMethod, isAsyncController)
                let routePrefixAttributes = GetRoutePrefixAttributes(controllerType, actionMethod)
                // Each route attribute
                from routeAttribute in GetRouteAttributes(actionMethod)
                // Each prefix so that the route is prefixed with each one available.
                // Using DefaultIfEmpty to simulate the notion of a left-join with the route attribute.
                // Taking only the first if ignoring route prefixes because prefixes will have no effect.
                from routePrefixAttribute in routePrefixAttributes.DefaultIfEmpty(null).Take(routeAttribute.IgnoreRoutePrefix ? 1 : int.MaxValue)
                // Build the specification
                select new RouteSpecification
                {
                    ActionMethod = actionMethod,
                    ActionName = actionName,
                    ActionPrecedence = GetSortableOrder(routeAttribute.ActionPrecedence),
                    AreaName = GetAreaName(routeAreaAttribute, controllerType),
                    AreaUrl = GetAreaUrl(routeAreaAttribute, controllerType),
                    ControllerIndex = controllerIndex,
                    ControllerName = controllerType.GetControllerName(),
                    ControllerPrecedence = GetSortableOrder(routeAttribute.ControllerPrecedence),
                    ControllerType = controllerType,
                    HttpMethods = routeAttribute.HttpMethods,
                    IgnoreAreaUrl = routeAttribute.IgnoreAreaUrl,
                    IgnoreRoutePrefix = routeAttribute.IgnoreRoutePrefix,
                    IsAbsoluteUrl = routeAttribute.IsAbsoluteUrl,
                    PrefixPrecedence = GetSortableOrder(routePrefixAttribute.SafeGet(a => a.Precedence, int.MaxValue)),
                    RouteName = routeAttribute.RouteName,
                    RoutePrefixUrl = GetRoutePrefixUrl(routePrefixAttribute, controllerType),
                    RouteUrl = routeAttribute.RouteUrl ?? actionName,
                    SitePrecedence = GetSortableOrder(routeAttribute.SitePrecedence),
                };

            // Return specs ordered by route precedence: 
            return specs.OrderBy(x => x.SitePrecedence)
                        .ThenBy(x => x.ControllerIndex)
                        .ThenBy(x => x.PrefixPrecedence)
                        .ThenBy(x => x.ControllerPrecedence)
                        .ThenBy(x => x.ActionPrecedence);
        }

        private static string GetActionName(MethodInfo actionMethod, bool isAsyncController)
        {
            var actionName = actionMethod.Name;
            
            if (isAsyncController && actionName.EndsWith("Async"))
            {
                actionName = actionName.Substring(0, actionName.Length - 5);
            }
            
            return actionName;
        }

        private static IEnumerable<IRouteAttribute> GetRouteAttributes(MethodInfo actionMethod)
        {
            return actionMethod.GetCustomAttributes<IRouteAttribute>(false);
        }

        private static RouteAreaAttribute GetRouteAreaAttribute(Type controllerType)
        {
            return controllerType.GetCustomAttribute<RouteAreaAttribute>(true);
        }

        private static string GetAreaName(RouteAreaAttribute routeAreaAttribute, Type controllerType)
        {
            if (routeAreaAttribute == null)
            {
                return null;
            }

            // If given an area name, then use it.
            // Otherwise, use the last section of the namespace of the controller, as a convention.
            return routeAreaAttribute.AreaName ?? controllerType.GetConventionalAreaName();
        }

        private static string GetAreaUrl(RouteAreaAttribute routeAreaAttribute, Type controllerType)
        {
            if (routeAreaAttribute == null)
            {
                return null;
            }

            if (routeAreaAttribute.AreaUrl.HasNoValue())
            {
                return null;
            }

            // If we're given an area url or an area name, then use it.
            // Otherwise, get the area name from the namespace of the controller, as a convention.
            var areaUrlOrName = routeAreaAttribute.AreaUrl ?? routeAreaAttribute.AreaName;
            return areaUrlOrName ?? controllerType.GetConventionalAreaName();
        }

        private static IEnumerable<RoutePrefixAttribute> GetRoutePrefixAttributes(Type controllerType, MethodInfo actionMethod)
        {
            return controllerType.GetCustomAttributes<RoutePrefixAttribute>(true).ToList();
        }

        private static string GetRoutePrefixUrl(RoutePrefixAttribute routePrefixAttribute, Type controllerType)
        {
            if (routePrefixAttribute == null)
            {
                return null;
            }

            // If we're given route prefix url, use it.
            // Otherwise, use the controller name as a convention.
            return routePrefixAttribute.Url ?? controllerType.GetControllerName();
        }

        /// <summary>
        /// Gets the sortable order for the given value.
        /// </summary>
        /// <param name="value">An integer sort order, where positive N is Nth, and negative N is Nth from last.</param>
        /// <returns>The sortable order corresponding to the given value.</returns>
        private static long GetSortableOrder(int value)
        {
            return (value >= 0 ? long.MinValue : long.MaxValue) + value;
        }
    }
}