using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Mvc.Constraints
{
    public class InboundHttpMethodConstraint : HttpMethodConstraint, IInboundHttpMethodConstraint
    {
        /// <summary>
        /// Constrains an inbound route by HTTP method.
        /// </summary>
        public InboundHttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods)
        {
        }

        protected override bool Match(HttpContextBase httpContext, Route route, string parameterName,
                                      RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
                return true;

            // Make sure to check for HTTP method overrides!
            var httpMethod = httpContext.Request.GetHttpMethodOverride();

            return AllowedMethods.Any(m => m.ValueEquals(httpMethod));
        }
    }
}