using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Http.SelfHost.Framework;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost {
    public class HttpAttributeRoutingConfiguration : AttributeRoutingConfiguration<IHttpController, AttributeRoute, RouteParameter, HttpRequestMessage, IHttpRouteData>
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IConstraintFactory _constraintFactory;
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
        public override IConstraintFactory ConstraintFactory {
            get { return _constraintFactory; }
        }

        /// <summary>
        /// Parameter factory
        /// </summary>
        public override IParameterFactory<RouteParameter> ParameterFactory {
            get { return _parameterFactory; }
        }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IHttpRouteConstraint constraint) {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }
    }
}
