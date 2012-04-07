using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.WebHost;
using System.Web.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost {
    public class HttpAttributeRoutingConfiguration : WebAttributeRoutingConfiguration<IHttpController, RouteParameter>
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IParameterFactory<RouteParameter> _parameterFactory;

        public HttpAttributeRoutingConfiguration()
            : base(() => HttpControllerRouteHandler.Instance)
        {
            _attributeFactory = new HttpAttributeRouteFactory(this);
            _parameterFactory = new RouteParameterFactory();
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
        public override IParameterFactory<RouteParameter> ParameterFactory {
            get { return _parameterFactory; }
        }
    }
}
