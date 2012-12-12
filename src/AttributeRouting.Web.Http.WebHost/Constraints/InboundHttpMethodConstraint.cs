using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.WebHost.Constraints
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

            return base.Match(httpContext, route, parameterName, values, routeDirection);
        }
    }
}