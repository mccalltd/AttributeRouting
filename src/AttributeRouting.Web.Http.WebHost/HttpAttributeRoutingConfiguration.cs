using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.WebHost;
using System.Web.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost {
    public class HttpAttributeRoutingConfiguration : WebAttributeRoutingConfiguration
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IParameterFactory _parameterFactory;

        public HttpAttributeRoutingConfiguration()
            : base(() => HttpControllerRouteHandler.Instance)
        {
            _attributeFactory = new HttpAttributeRouteFactory(this);
            _parameterFactory = new RouteParameterFactory();
        }

        public override Type FrameworkControllerType {
            get { return typeof (IHttpController); }
        }

        /// <summary>
        /// Attribute factory
        /// </summary>
        public override IAttributeRouteFactory AttributeFactory {
            get { return _attributeFactory; }
        }

        /// <summary>
        /// Parameter factory
        /// </summary>
        public override IParameterFactory ParameterFactory {
            get { return _parameterFactory; }
        }

        /// <summary>
        /// Scans the assembly of the specified controller for routes to register.
        /// </summary>
        /// <typeparam name="T">The type of the controller used to specify the assembly</typeparam>
        public void ScanAssemblyOf<T>() where T : IHttpController {
            ScanAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Adds all the routes for the specified controller type to the end of the route collection.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        public void AddRoutesFromController<T>() where T : IHttpController {
            AddRoutesFromController(typeof(T));
        }

        /// <summary>
        /// Adds all the routes for all the controllers that derive from the specified controller
        /// to the end of the route collection.
        /// </summary>
        /// <typeparam name="T">The base controller type</typeparam>
        public void AddRoutesFromControllersOfType<T>() where T : IHttpController {
            AddRoutesFromControllersOfType(typeof(T));
        }
    }
}
