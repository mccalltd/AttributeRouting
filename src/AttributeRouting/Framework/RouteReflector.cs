using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// A reflector that inspects the assemblies provided in configuration to find AttributeRouting attributes.
    /// </summary>    
    public class RouteReflector
    {
        private readonly AttributeRoutingConfigurationBase _configuration;

        public RouteReflector(AttributeRoutingConfigurationBase configuration)
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

            var scannedControllerTypes = _configuration.Assemblies.SelectMany(a => a.GetControllerTypes(_configuration.FrameworkControllerType)).ToList();
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
                    let convention = controllerType.GetCustomAttribute<RouteConventionAttributeBase>(false)
                    let routeAreaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true)
                    let routePrefixAttribute = controllerType.GetCustomAttribute<RoutePrefixAttribute>(true)
                    from actionMethod in controllerType.GetActionMethods(inheritActionsFromBaseController)
                    from routeAttribute in GetRouteAttributes(actionMethod, convention)
                    orderby controllerIndex , routeAttribute.Precedence
                    // precedence is within a controller
                    let routeName = routeAttribute.RouteName
                    let subdomain = GetAreaSubdomain(routeAreaAttribute)
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
                        ActionName = actionMethod.Name,
                        ActionParameters = actionMethod.GetParameters(),
                        RouteUrl = routeAttribute.RouteUrl,
                        RouteUrlTranslationKey = routeAttribute.TranslationKey,
                        HttpMethods = routeAttribute.HttpMethods,
                        DefaultAttributes = GetDefaultAttributes(actionMethod, routeName, convention),
                        ConstraintAttributes = GetConstraintAttributes(actionMethod, routeName, convention),
                        RouteName = routeName,
                        IsAbsoluteUrl = routeAttribute.IsAbsoluteUrl,
                        UseLowercaseRoute = routeAttribute.UseLowercaseRouteFlag,
                        PreserveCaseForUrlParameters = routeAttribute.PreserveCaseForUrlParametersFlag,
                        AppendTrailingSlash = routeAttribute.AppendTrailingSlashFlag
                    }).ToList();
        }

        private static IEnumerable<RouteAttributeBase> GetRouteAttributes(MethodInfo actionMethod, RouteConventionAttributeBase convention)
        {
            var attributes = new List<RouteAttributeBase>();

            // Add convention-based attributes
            if (convention != null)
                attributes.AddRange(convention.GetRouteAttributes(actionMethod));

            // Add explicitly-defined attributes
            attributes.AddRange(actionMethod.GetCustomAttributes<RouteAttributeBase>(false));

            return attributes.OrderBy(a => a.Order);
        }

        private static string GetAreaUrl(RouteAreaAttribute routeAreaAttribute, string subdomain)
        {
            if (routeAreaAttribute == null)
                return null;

            // If a subdomain is specified for the area either in the RouteAreaAttribute or via 
            // configuration, then assume the area url is blank;
            // eg: admin.badass.com.
            // However, our fearless coder can decide to explicitly specify an area url if desired;
            // eg: internal.badass.com/admin.
            if (subdomain.HasValue() && routeAreaAttribute.AreaUrl.HasNoValue())
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

        private static ICollection<RouteDefaultAttribute> GetDefaultAttributes(MethodInfo actionMethod, string routeName, RouteConventionAttributeBase convention)
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

        private static ICollection<RouteConstraintAttributeBase> GetConstraintAttributes(MethodInfo actionMethod, string routeName, RouteConventionAttributeBase convention)
        {
            var constraintAttributes = new List<RouteConstraintAttributeBase>();

            // Yield explicitly defined constraint attributes first
            constraintAttributes.AddRange(
                from constraintAttribute in actionMethod.GetCustomAttributes<RouteConstraintAttributeBase>(false)
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