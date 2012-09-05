using System.Web.Http.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost
{
    public class HttpAttributeRoutingConfiguration : HttpAttributeRoutingConfigurationBase
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IRouteConstraintFactory _routeConstraintFactory;
        private readonly IParameterFactory _parameterFactory;

        public HttpAttributeRoutingConfiguration()
        {
            _attributeFactory = new AttributeRouteFactory(this);
            _routeConstraintFactory = new RouteConstraintFactory(this);
            _parameterFactory = new RouteParameterFactory();

            RegisterDefaultInlineRouteConstraints<IHttpRouteConstraint>(typeof(RegexRouteConstraintAttribute).Assembly);
        }

        /// <summary>
        /// Attribute factory
        /// </summary>
        public override IAttributeRouteFactory AttributeFactory
        {
            get { return _attributeFactory; }
        }

        /// <summary>
        /// Constraint factory
        /// </summary>
        public override IRouteConstraintFactory RouteConstraintFactory
        {
            get { return _routeConstraintFactory; }
        }

        /// <summary>
        /// Parameter factory
        /// </summary>
        public override IParameterFactory ParameterFactory
        {
            get { return _parameterFactory; }
        }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IHttpRouteConstraint constraint)
        {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }
    }
}