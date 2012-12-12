using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Helpers;

namespace AttributeRouting.Web.Constraints
{
    public class RestfulHttpMethodConstraint : HttpMethodConstraint, IRestfulHttpMethodConstraint
    {
        /// <summary>
        /// Constrains a route by HTTP method.
        /// </summary>
        public RestfulHttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods)
        {
        }

        protected override bool Match(HttpContextBase httpContext, Route route, string parameterName,
                                      RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (routeDirection == RouteDirection.UrlGeneration)
                return true;

            var httpMethod = httpContext.Request.GetHttpMethod();

            return AllowedMethods.Any(m => m.ValueEquals(httpMethod));
        }
    }
}