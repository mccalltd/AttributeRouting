using System;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost.Framework;

namespace AttributeRouting.Web.Http.WebHost
{
    public class HttpWebConfiguration : HttpConfigurationBase
    {
        public HttpWebConfiguration(bool inMemory = false)
        {
            if (inMemory)
            {
                Init();
            }
            else
            {
                AttributeRouteFactory = new AttributeRouteFactory(this);
                ParameterFactory = new RouteParameterFactory();
                RouteConstraintFactory = new RouteConstraintFactory(this);

                RouteHandlerFactory = () => null;
                RegisterDefaultInlineRouteConstraints<IRouteConstraint>(
                    typeof(Web.Constraints.RegexRouteConstraint).Assembly);
            }
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

        /// <summary>
        /// Specifies a function that returns an alternate route handler.
        /// By default, the route handler is the default HttpControllerRouteHandler.
        /// </summary>
        /// <param name="routeHandlerFactory">The route handler to use.</param>
        public void UseRouteHandler(Func<IRouteHandler> routeHandlerFactory)
        {
            RouteHandlerFactory = routeHandlerFactory;
        }

        internal Func<IRouteHandler> RouteHandlerFactory { get; set; }
    }
}
