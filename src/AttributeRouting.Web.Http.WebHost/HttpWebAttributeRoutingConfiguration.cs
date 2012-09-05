using System;
using System.Web.Http.WebHost;
using System.Web.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost
{
    public class HttpWebAttributeRoutingConfiguration : HttpAttributeRoutingConfigurationBase
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IParameterFactory _parameterFactory;
        private readonly RouteConstraintFactory _routeConstraintFactory;

        public HttpWebAttributeRoutingConfiguration()
        {
            _attributeFactory = new AttributeRouteFactory(this);
            _parameterFactory = new RouteParameterFactory();
            _routeConstraintFactory = new RouteConstraintFactory(this);

            RouteHandlerFactory = () => HttpControllerRouteHandler.Instance;

            RegisterDefaultInlineRouteConstraints<IRouteConstraint>(typeof(Web.Constraints.RegexRouteConstraint).Assembly);
        }

        public Func<IRouteHandler> RouteHandlerFactory { get; set; }

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
        /// By default, the route handler is the default HttpControllerRouteHandler.
        /// </summary>
        /// <param name="routeHandlerFactory">The route handler to use.</param>
        public void UseRouteHandler(Func<IRouteHandler> routeHandlerFactory)
        {
            RouteHandlerFactory = routeHandlerFactory;
        }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IRouteConstraint constraint)
        {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }
    }
}
