using System;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost
{
    public class HttpWebAttributeRoutingConfiguration : HttpAttributeRoutingConfigurationBase
    {
        public HttpWebAttributeRoutingConfiguration()
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            ParameterFactory = new RouteParameterFactory();
            RouteConstraintFactory = new RouteConstraintFactory(this);

            RouteHandlerFactory = () => null;
            RegisterDefaultInlineRouteConstraints<IRouteConstraint>(typeof(Web.Constraints.RegexRouteConstraint).Assembly);
        }

        public Func<IRouteHandler> RouteHandlerFactory { get; set; }

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
