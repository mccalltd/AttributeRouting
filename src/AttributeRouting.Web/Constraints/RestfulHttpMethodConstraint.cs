using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Helpers;

namespace AttributeRouting.Web.Constraints
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    /// <remarks>
    /// AttributeRouting uses its own implementation of the Match method 
    /// to handle X-HTTP-Method-Override values in the request headers, form, and query collections.
    /// </remarks>
    public class RestfulHttpMethodConstraint : HttpMethodConstraint, IRestfulHttpMethodConstraint
    {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        public RestfulHttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods) { }

        protected override bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
                return true;

            var httpMethod = httpContext.Request.GetHttpMethod();

            return AllowedMethods.Any(m => m.ValueEquals(httpMethod));
        }
    }
}