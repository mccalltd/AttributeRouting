using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Mvc.Helpers;

namespace AttributeRouting.Mvc
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    public class RestfulHttpMethodConstraint : RestfulHttpMethodConstraintBase, IRouteConstraint
    {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        public RestfulHttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods) { }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
                                      RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(routeDirection == RouteDirection.UrlGeneration, httpContext.Request.GetHttpMethod());
        }
    }
}