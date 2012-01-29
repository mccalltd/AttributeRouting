using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Extensions;

namespace AttributeRouting
{
    /// <summary>
    /// Configuration options to use when mapping AttributeRoutes.
    /// </summary>
    public class AttributeRoutingConfiguration
    {
        /// <summary>
        /// Creates and initializes a new configuration object.
        /// </summary>
        public AttributeRoutingConfiguration()
        {
            UseLowercaseRoutes = false;
            DefaultRouteConstraints = new Dictionary<string, IRouteConstraint>();
            RouteHandlerFactory = (() => new System.Web.Mvc.MvcRouteHandler());

            Assemblies = new List<Assembly>();
            PromotedControllerTypes = new List<Type>();
        }

        internal List<Assembly> Assemblies { get; set; }
        internal List<Type> PromotedControllerTypes { get; set; }
        internal IDictionary<string, IRouteConstraint> DefaultRouteConstraints { get; set; }

        internal Func<IRouteHandler> RouteHandlerFactory { get; set; }

        /// <summary>
        /// When true, the generated routes will produce lowercase outbound URLs.
        /// The default is false.
        /// </summary>
        public bool UseLowercaseRoutes { get; set; }

        /// <summary>
        /// When true, the generated routes will have auto-generated route names in the form controller_action.
        /// The default is false.
        /// </summary>
        public bool AutoGenerateRouteNames { get; set; }

        /// <summary>
        /// Scans the assembly of the specified controller for routes to register.
        /// </summary>
        /// <typeparam name="TController">The controller type used to specify the assembly</typeparam>
        public void ScanAssemblyOf<TController>()
        {
            ScanAssembly(typeof(TController).Assembly);
        }

        /// <summary>
        /// Scans the specified assembly for routes to register.
        /// </summary>
        /// <param name="assembly">The assembly</param>
        public void ScanAssembly(Assembly assembly)
        {
            if (!Assemblies.Contains(assembly))
                Assemblies.Add(assembly);
        }

        /// <summary>
        /// Adds all the routes for all the controllers that derive from the specified controller
        /// to the end of the route collection.
        /// </summary>
        /// <typeparam name="TController">The base controller type</typeparam>
        public void AddRoutesFromControllersOfType<TController>()
        {
            AddRoutesFromControllersOfType(typeof(TController));
        }

        /// <summary>
        /// Adds all the routes for all the controllers that derive from the specified controller
        /// to the end of the route collection.
        /// </summary>
        /// <param name="baseControllerType">The base controller type</param>
        public void AddRoutesFromControllersOfType(Type baseControllerType)
        {
            var assembly = baseControllerType.Assembly;

            var controllerTypes = from controllerType in assembly.GetControllerTypes()
                                  where baseControllerType.IsAssignableFrom(controllerType)
                                  select controllerType;

            foreach (var controllerType in controllerTypes)
                AddRoutesFromController(controllerType);
        }

        /// <summary>
        /// Adds all the routes for the specified controller type to the end of the route collection.
        /// </summary>
        /// <typeparam name="TController">The controller type</typeparam>
        public void AddRoutesFromController<TController>()
            where TController : IController
        {
            AddRoutesFromController(typeof(TController));
        }

        /// <summary>
        /// Adds all the routes for the specified controller type to the end of the route collection.
        /// </summary>
        /// <param name="controllerType">The controller type</param>
        public void AddRoutesFromController(Type controllerType)
        {
            if (!PromotedControllerTypes.Contains(controllerType))
                PromotedControllerTypes.Add(controllerType);
        }

        /// <summary>
        /// When using AddRoutesFromControllersOfType or AddRoutesFromController to set the precendence of the routes,
        /// you must explicitly specify that you want to include the remaining routes discoved while scanning assemblies.
        /// </summary>
        [Obsolete("This is a vestigial workaround for an old assembly scanning issue.")]
        public void AddTheRemainingScannedRoutes() { }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IRouteConstraint constraint)
        {
            if (!DefaultRouteConstraints.ContainsKey(keyRegex))
                DefaultRouteConstraints.Add(keyRegex, constraint);
        }

        /// <summary>
        /// Specifies a function that returns an alternate route handler.
        /// By default, the route handler is the default MVC handler System.Web.Mvc.MvcRouteHandler()
        /// </summary>
        /// <example>
        /// <code>
        /// routes.MapAttributeRoutes(config =>
        /// {
        ///    config.ScanAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        ///    config.UseRouteHandler(() => new MyOtherLibrary.Mvc.CustomRouteHandler());
        ///    // default:  config.UseRouteHandler(() => new System.Web.Mvc.MvcRouteHandler());
        /// });
        /// </code>
        /// </example>
        /// <param name="routeHandlerFactory"></param>
        public void UseRouteHandler(Func<IRouteHandler> routeHandlerFactory)
        {
            this.RouteHandlerFactory = routeHandlerFactory;
        }
    }
}