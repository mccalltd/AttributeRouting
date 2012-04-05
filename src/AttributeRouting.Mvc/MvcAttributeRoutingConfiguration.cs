using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttributeRouting.Mvc {
    public class MvcAttributeRoutingConfiguration : AttributeRoutingConfiguration {

        public MvcAttributeRoutingConfiguration()
            : base(new MvcControllerAssemblyScanner())
        {
            RouteHandlerFactory = () => new MvcRouteHandler();
        }

        internal Func<IRouteHandler> RouteHandlerFactory { get; set; }

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
        public void UseRouteHandler(Func<IRouteHandler> routeHandlerFactory) {
            RouteHandlerFactory = routeHandlerFactory;
        }

        /// <summary>
        /// Adds all the routes for the specified controller type to the end of the route collection.
        /// </summary>
        /// <typeparam name="TController">The controller type</typeparam>
        public void AddRoutesFromController<TController>()
            where TController : IController {
            AddRoutesFromController(typeof(TController));
        }
    }
}
