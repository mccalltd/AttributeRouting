using System;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Mvc.Framework.Factories;

namespace AttributeRouting.Web.Mvc
{
    public class AttributeRoutingConfiguration : WebAttributeRoutingConfigurationBase
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IParameterFactory _parameterFactory;
        private readonly RouteConstraintFactory _routeConstraintFactory;

        public AttributeRoutingConfiguration()
        {
            _attributeFactory = new AttributeRouteFactory(this);
            _parameterFactory = new RouteParameterFactory();
            _routeConstraintFactory = new RouteConstraintFactory(this);

            RouteHandlerFactory = () => new MvcRouteHandler();
        }

        public Func<IRouteHandler> RouteHandlerFactory { get; set; }

        public override Type FrameworkControllerType
        {
            get { return typeof(IController); }
        }

        /// <summary>
        /// Attribute factory
        /// </summary>
        public override IAttributeRouteFactory AttributeFactory
        {
            get { return _attributeFactory; }
        }

        /// <summary>
        /// Parameter factory
        /// </summary>
        public override IParameterFactory ParameterFactory
        {
            get { return _parameterFactory; }
        }

        /// <summary>
        /// Constraint factory
        /// </summary>
        public override IRouteConstraintFactory RouteConstraintFactory
        {
            get { return _routeConstraintFactory; }
        }

        /// <summary>
        /// Specifies a function that returns an alternate route handler.
        /// By default, the route handler is the default MvcRouteHandler.
        /// </summary>
        /// <param name="routeHandlerFactory">The route hanlder to use.</param>
        public void UseRouteHandler(Func<IRouteHandler> routeHandlerFactory)
        {
            RouteHandlerFactory = routeHandlerFactory;
        }

        /// <summary>
        /// Scans the assembly of the specified controller for routes to register.
        /// </summary>
        /// <typeparam name="T">The type of the controller used to specify the assembly</typeparam>
        public void ScanAssemblyOf<T>() where T : IController
        {
            ScanAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Adds all the routes for the specified controller type to the end of the route collection.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        public void AddRoutesFromController<T>() where T : IController
        {
            AddRoutesFromController(typeof(T));
        }

        /// <summary>
        /// Adds all the routes for all the controllers that derive from the specified controller
        /// to the end of the route collection.
        /// </summary>
        /// <typeparam name="T">The base controller type</typeparam>
        public void AddRoutesFromControllersOfType<T>() where T : IController
        {
            AddRoutesFromControllersOfType(typeof(T));
        }
    }
}
