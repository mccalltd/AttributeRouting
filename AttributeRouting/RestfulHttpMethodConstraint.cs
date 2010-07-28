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
            object httpMethod;

            if (routeDirection == RouteDirection.UrlGeneration)
            {
                if (!values.TryGetValue(parameterName, out httpMethod))
                    httpMethod = "GET";
            }
            else
                httpMethod = httpContext.Request.GetHttpMethod();

            var match = AllowedMethods.Any(m => m.Equals((string)httpMethod, StringComparison.OrdinalIgnoreCase));

            return match;
        }
    }
}