using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Http.SelfHost.Framework;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost {
    public class HttpAttributeRoutingConfiguration : AttributeRoutingConfiguration<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter, HttpRequestMessage, IHttpRouteData>
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IConstraintFactory<IHttpRouteConstraint> _constraintFactory;
        private readonly IParameterFactory<RouteParameter> _parameterFactory;

        public HttpAttributeRoutingConfiguration() {
            _attributeFactory = new AttributeRouteFactory(this);
            _constraintFactory = new HttpRouteConstraintFactory();
            _parameterFactory = new RouteParameterFactory();
        }

        /// <summary>
        /// Attribute factory
        /// </summary>
        public override IAttributeRouteFactory AttributeFactory {
            get { return _attributeFactory; }
        }

        /// <summary>
        /// Constraint factory
        /// </summary>
        public override IConstraintFactory<IHttpRouteConstraint> ConstraintFactory {
            get { return _constraintFactory; }
        }

        /// <summary>
        /// Parameter factory
        /// </summary>
        public override IParameterFactory<RouteParameter> ParameterFactory {
            get { return _parameterFactory; }
        }
    }
}
