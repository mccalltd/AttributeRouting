using System.Web;
using System.Web.Routing;
using AttributeRouting.AspNet.Helpers;
using AttributeRouting.Constraints;

namespace AttributeRouting.AspNet.Constraints
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    public class HttpMethodConstraint : HttpMethodConstraintBase, IRouteConstraint
    {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        public HttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods) { }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
                                      RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(routeDirection == RouteDirection.UrlGeneration, httpContext.Request.GetHttpMethod());
        }
    }
}