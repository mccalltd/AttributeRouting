using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// A static factory to help create route reflectors
    /// </summary>
    public static class RouteReflectorFactory {

        /// <summary>
        /// Creates a new RouteReflector for the given types
        /// </summary>
        /// <typeparam name="TConstraint"></typeparam>
        /// <typeparam name="TController"></typeparam>
        /// <typeparam name="TRoute"></typeparam>
        /// <typeparam name="TRouteParameter"></typeparam>
        /// <typeparam name="TRequestContext"></typeparam>
        /// <typeparam name="TRouteData"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static RouteReflector<TController, TRoute, TRouteParameter, TRequestContext, TRouteData> Create<TController, TRoute, TRouteParameter, TRequestContext, TRouteData>(
            AttributeRoutingConfiguration<TController, TRoute, TRouteParameter, TRequestContext, TRouteData> configuration) {
            return new RouteReflector<TController, TRoute, TRouteParameter, TRequestContext, TRouteData>(configuration);
        }
    }

    /// <summary>
    /// A reflector that inspects the assemblies provided in configuration to find attribute routes and constraints
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    /// <typeparam name="TRoute"></typeparam>
    /// <typeparam name="TRouteParameter"></typeparam>
    /// <typeparam name="TRequestContext"></typeparam>
    /// <typeparam name="TRouteData"></typeparam>
    public class RouteReflector<TController, TRoute, TRouteParameter, TRequestContext, TRouteData>
    {
        private readonly AttributeRoutingConfiguration<TController, TRoute, TRouteParameter, TRequestContext, TRouteData> _configuration;

        internal RouteReflector(AttributeRoutingConfiguration<TController, TRoute, TRouteParameter, TRequestContext, TRouteData> configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        public IEnumerable<RouteSpecification> GenerateRouteSpecifications()
        {
            var controllerRouteSpecs = GenerateRouteSpecifications(_configuration.PromotedControllerTypes, _configuration.InheritActionsFromBaseController);
            foreach (var spec in controllerRouteSpecs)
                yield return spec;

            if (!_configuration.Assemblies.Any())
                yield break;

            var scannedControllerTypes = _configuration.Assemblies.SelectMany(a => a.GetControllerTypes<TController>()).ToList();
            var remainingControllerTypes = scannedControllerTypes.Except(_configuration.PromotedControllerTypes);

            var remainingRouteSpecs = GenerateRouteSpecifications(remainingControllerTypes, _configuration.InheritActionsFromBaseController);

            foreach (var spec in remainingRouteSpecs)
                yield return spec;
        }

        private IEnumerable<RouteSpecification> GenerateRouteSpecifications(IEnumerable<Type> controllerTypes, bool inheritActionsFromBaseController)
        {
            var controllerCount = 0;

            return (from controllerType in controllerTypes
                    let controllerIndex = controllerCount++
                    let convention = controllerType.GetCustomAttribute<IRouteConvention>(false)
                    let routeAreaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true)
                    let routePrefixAttribute = controllerType.GetCustomAttribute<RoutePrefixAttribute>(true)
                    from actionMethod in controllerType.GetActionMethods(inheritActionsFromBaseController)
                    from routeAttribute in GetRouteAttributes(actionMethod, convention)
                    orderby controllerIndex, routeAttribute.Precedence
                    // precedence is within a controller
                    let routeName = routeAttribute.RouteName
                    select new RouteSpecification
                    {
                        AreaName = routeAreaAttribute.SafeGet(a => a.AreaName),
                        AreaUrl = GetAreaUrl(routeAreaAttribute),
                        AreaUrlTranslationKey = routeAreaAttribute.SafeGet(a => a.TranslationKey),
                        Subdomain = GetAreaSubdomain(routeAreaAttribute),
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

        private static IEnumerable<IRouteAttribute> GetRouteAttributes(MethodInfo actionMethod,
                                                                      IRouteConvention convention)
        {
            var attributes = new List<IRouteAttribute>();

            // Add convention-based attributes
            if (convention != null)
                attributes.AddRange(convention.GetRouteAttributes(actionMethod));

            // Add explicitly-defined attributes
            attributes.AddRange(actionMethod.GetCustomAttributes<IRouteAttribute>(false));

            return attributes.OrderBy(a => a.Order);
        }

        private static string GetAreaUrl(RouteAreaAttribute routeAreaAttribute)
        {
            if (routeAreaAttribute == null)
                return null;

            // If a subdomain is specified for the area, then assume the area url is blank;
            // eg: admin.badass.com.
            // However, our fearless coder can decide to explicitly specify an area url if desired;
            // eg: internal.badass.com/admin.
            if (routeAreaAttribute.Subdomain.HasValue() && routeAreaAttribute.AreaUrl.HasNoValue())
                return null;

            return routeAreaAttribute.AreaUrl ?? routeAreaAttribute.AreaName;
        }

        private string GetAreaSubdomain(RouteAreaAttribute routeAreaAttribute)
        {
            if (routeAreaAttribute == null)
                return null;

            // Check for a subdomain ovveride specified via configuration object.
            var subdomainOverride = (from o in _configuration.AreaSubdomainOverrides
                                     where o.Key == routeAreaAttribute.AreaName
                                     select o.Value).FirstOrDefault();

            if (subdomainOverride != null)
                return subdomainOverride;

            return routeAreaAttribute.Subdomain;
        }

        private static string GetRoutePrefix(RoutePrefixAttribute routePrefixAttribute, MethodInfo actionMethod, IRouteConvention convention)
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
                                                                               IRouteConvention convention)
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

        private static ICollection<IAttributeRouteConstraint> GetConstraintAttributes(MethodInfo actionMethod,
                                                                                     string routeName,
                                                                                     IRouteConvention convention)
        {
            var constraintAttributes = new List<IAttributeRouteConstraint>();

            // Yield explicitly defined constraint attributes first
            constraintAttributes.AddRange(
                from constraintAttribute in actionMethod.GetCustomAttributes<IAttributeRouteConstraint>(false)
                where !constraintAttribute.ForRouteNamed.HasValue() ||
                      constraintAttribute.ForRouteNamed == routeName
                select constraintAttribute);

            // Yield convention-based constraints next
            if (convention != null)
                constraintAttributes.AddRange(convention.GetRouteConstraintAttributes(actionMethod));

            return constraintAttributes.ToList();
        }
    }
}