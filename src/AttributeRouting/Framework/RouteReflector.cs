using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Creates <see cref="RouteSpecification"/> objects according to the 
    /// options set in implementations of <see cref="AttributeRoutingConfigurationBase"/>.
    /// </summary>    
    public class RouteReflector
    {
        private readonly AttributeRoutingConfigurationBase _configuration;

        public RouteReflector(AttributeRoutingConfigurationBase configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        /// <summary>
        /// Yields all the information needed by <see cref="RouteBuilder"/> to build routes.
        /// </summary>
        public IEnumerable<RouteSpecification> BuildRouteSpecifications()
        {
            var controllerRouteSpecs = BuildRouteSpecifications(_configuration.OrderedControllerTypes);
            foreach (var spec in controllerRouteSpecs)
                yield return spec;

            if (!_configuration.Assemblies.Any())
                yield break;

            var scannedControllerTypes = _configuration.Assemblies.SelectMany(a => a.GetControllerTypes(_configuration.FrameworkControllerType)).ToList();
            var unspecdControllerTypes = scannedControllerTypes.Except(_configuration.OrderedControllerTypes);
            var scannedRouteSpecs = BuildRouteSpecifications(unspecdControllerTypes);

            foreach (var spec in scannedRouteSpecs)
                yield return spec;
        }

        /// <summary>
        /// Builds the reoute specifications for the given controller types.
        /// </summary>
        /// <param name="controllerTypes">The controller type.</param>
        /// <returns>An enumerable of <see cref="RouteSpecification"/>.</returns>
        private IEnumerable<RouteSpecification> BuildRouteSpecifications(IEnumerable<Type> controllerTypes)
        {
            var controllerCount = 0; // needed to increment controller index
            var inheritActionsFromBaseController = _configuration.InheritActionsFromBaseController;

            return (from controllerType in controllerTypes
                    let controllerIndex = controllerCount++
                    let convention = controllerType.GetCustomAttribute<RouteConventionAttributeBase>(false)
                    let routeAreaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true)
                    let routePrefixAttribute = controllerType.GetCustomAttribute<RoutePrefixAttribute>(true)
                    from actionMethod in controllerType.GetActionMethods(inheritActionsFromBaseController)
                    from routeAttribute in GetRouteAttributes(actionMethod, convention)
                    let routeName = routeAttribute.RouteName
                    let subdomain = GetAreaSubdomain(routeAreaAttribute)
                    let isAsyncController = controllerType.IsAsyncController()
                    // SitePrecedence > controllerIndex > Precedence
                    let sitePrecedence = GetSortableOrder(routeAttribute.SitePrecedence)
                    let controllerPrecedence = GetSortableOrder(routeAttribute.Precedence)
                    orderby sitePrecedence , controllerIndex , controllerPrecedence
                    select new RouteSpecification
                    {
                        AreaName = routeAreaAttribute.SafeGet(a => a.AreaName),
                        AreaUrl = GetAreaUrl(routeAreaAttribute, subdomain),
                        AreaUrlTranslationKey = routeAreaAttribute.SafeGet(a => a.TranslationKey),
                        Subdomain = subdomain,
                        RoutePrefixUrl = GetRoutePrefix(routePrefixAttribute, actionMethod, convention),
                        RoutePrefixUrlTranslationKey = routePrefixAttribute.SafeGet(a => a.TranslationKey),
                        ControllerType = controllerType,
                        ControllerName = controllerType.GetControllerName(),
                        ActionName = GetActionName(actionMethod, isAsyncController),
                        RouteUrl = routeAttribute.RouteUrl,
                        RouteUrlTranslationKey = routeAttribute.TranslationKey,
                        HttpMethods = routeAttribute.HttpMethods,
                        RouteName = routeName,
                        IsAbsoluteUrl = routeAttribute.IsAbsoluteUrl,
                        UseLowercaseRoute = routeAttribute.UseLowercaseRouteFlag,
                        PreserveCaseForUrlParameters = routeAttribute.PreserveCaseForUrlParametersFlag,
                        AppendTrailingSlash = routeAttribute.AppendTrailingSlashFlag
                    }).ToList();
        }

        /// <summary>
        /// Gets the action name.
        /// </summary>
        /// <param name="actionMethod">The <see cref="MethodInfo"/> for the action.</param>
        /// <param name="isAsyncController">True if the action's controller is an async controller.</param>
        /// <returns>The name of the action.</returns>
        private static string GetActionName(MethodInfo actionMethod, bool isAsyncController)
        {
            var actionName = actionMethod.Name;
            
            if (isAsyncController && actionName.EndsWith("Async"))
                actionName = actionName.Substring(0, actionName.Length - 5);
            
            return actionName;
        }

        /// <summary>
        /// Gets the route attributes for an action.
        /// </summary>
        /// <param name="actionMethod">The <see cref="MethodInfo"/> for the action.</param>
        /// <param name="convention">The <see cref="RouteConventionAttributeBase"/> applied to the action's controller.</param>
        /// <returns>The route attributes for the action.</returns>
        private static IEnumerable<IRouteAttribute> GetRouteAttributes(MethodInfo actionMethod, RouteConventionAttributeBase convention)
        {
            var attributes = new List<IRouteAttribute>();

            // Add convention-based attributes
            if (convention != null)
                attributes.AddRange(convention.GetRouteAttributes(actionMethod));

            // Add explicitly-defined attributes
            attributes.AddRange(actionMethod.GetCustomAttributes<IRouteAttribute>(false));

            return attributes.OrderBy(a => GetSortableOrder(a.Order));
        }

        /// <summary>
        /// Gets the area URL prefix.
        /// </summary>
        /// <param name="routeAreaAttribute">The <see cref="RouteAreaAttribute"/> for the controller.</param>
        /// <param name="subdomain">The configured subdomain for the area.</param>
        /// <returns>The URL prefix for the area.</returns>
        private static string GetAreaUrl(RouteAreaAttribute routeAreaAttribute, string subdomain)
        {
            if (routeAreaAttribute == null)
                return null;

            // If a subdomain is specified for the area either in the RouteAreaAttribute 
            // or via configuration, then assume the area url is blank; eg: admin.badass.com.
            // However, our fearless coder can decide to explicitly specify an area url if desired;
            // eg: internal.badass.com/admin.
            if (subdomain.HasValue() && routeAreaAttribute.AreaUrl.HasNoValue())
                return null;

            return routeAreaAttribute.AreaUrl ?? routeAreaAttribute.AreaName;
        }

        /// <summary>
        /// Gets the configured subdomain for an area.
        /// </summary>
        /// <param name="routeAreaAttribute">The <see cref="RouteAreaAttribute"/> for the controller.</param>
        /// <returns>The subdomain name configured for the area.</returns>
        private string GetAreaSubdomain(RouteAreaAttribute routeAreaAttribute)
        {
            if (routeAreaAttribute == null)
                return null;

            // Check for a subdomain override specified via configuration object.
            var subdomainOverride = (from o in _configuration.AreaSubdomainOverrides
                                     where o.Key == routeAreaAttribute.AreaName
                                     select o.Value).FirstOrDefault();

            if (subdomainOverride != null)
                return subdomainOverride;

            return routeAreaAttribute.Subdomain;
        }

        /// <summary>
        /// Gets a controller's route prefix URL.
        /// </summary>
        /// <param name="routePrefixAttribute">The <see cref="RoutePrefixAttribute"/> for the controller.</param>
        /// <param name="actionMethod">The <see cref="MethodInfo"/> for an action.</param>
        /// <param name="convention">The <see cref="RouteConventionAttributeBase"/> for the controller.</param>
        /// <returns>The route prefix URL to apply against the action.</returns>
        private static string GetRoutePrefix(RoutePrefixAttribute routePrefixAttribute, MethodInfo actionMethod, RouteConventionAttributeBase convention)
        {
            // Return an explicitly defined route prefix, if defined
            if (routePrefixAttribute != null)
                return routePrefixAttribute.Url;

            // Otherwise, if this is a convention-based controller, get the convention-based prefix
            if (convention != null)
                return convention.GetDefaultRoutePrefix(actionMethod);

            return null;
        }

        /// <summary>
        /// Gets the sortable order for the given value.
        /// </summary>
        /// <param name="value">An integer sort order, where 0 is undefined, positive N is Nth, and negative N is Nth from last.</param>
        /// <returns>The sortable order corresponding to the given value.</returns>
        private static int GetSortableOrder(int value)
        {
            return value == 0 ? 0 : (value > 0 ? int.MinValue : int.MaxValue) + value;
        }
    }
}