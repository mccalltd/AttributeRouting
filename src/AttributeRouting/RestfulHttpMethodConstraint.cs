using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Helpers;

namespace AttributeRouting
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    public class RestfulHttpMethodConstraint : HttpMethodConstraint
    {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        public RestfulHttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods) {}

        protected override bool Match(HttpContextBase httpContext, Route route, string parameterName,
                                      RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
                return true;
            
            var httpMethod = httpContext.Request.GetHttpMethod();

            return AllowedMethods.Any(m => m.Equals(httpMethod, StringComparison.OrdinalIgnoreCase));
        }
    }
}