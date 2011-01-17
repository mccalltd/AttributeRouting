using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting
{
    public class RestfulHttpMethodConstraint : HttpMethodConstraint
    {
        public RestfulHttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods) {}

        protected override bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
                return true;
            
            var httpMethod = httpContext.Request.GetHttpMethod();

            return AllowedMethods.Any(m => m.Equals(httpMethod, StringComparison.OrdinalIgnoreCase));
        }
    }
}