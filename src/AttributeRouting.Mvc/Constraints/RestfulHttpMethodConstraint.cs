using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Mvc.Helpers;

namespace AttributeRouting.Mvc
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    public class RestfulHttpMethodConstraint : HttpMethodConstraint, IRestfulHttpMethodConstraint {
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

            return Enumerable.Any<string>(AllowedMethods, m => m.Equals(httpMethod, StringComparison.OrdinalIgnoreCase));
        }
    }
}