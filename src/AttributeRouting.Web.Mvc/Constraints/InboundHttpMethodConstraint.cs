using System.Linq;
using System.Web;
using System.Web.Helpers;
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

        protected override bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
                return true;

            // Make sure to check for HTTP method overrides!
            var httpMethod = GetUnvalidatedHttpMethodOverride(httpContext.Request);

            return AllowedMethods.Any(m => m.ValueEquals(httpMethod));
        }

        /// <remarks>
        /// The reason we have our own is to provide support for System.Web.Helpers.Validation.Unvalidated calls.
        /// NOTE: This won't be needed once AR targets .NET 4.5.
        /// </remarks>
        private static string GetUnvalidatedHttpMethodOverride(HttpRequestBase request)
        {
            var httpMethod = request.HttpMethod;

            // If not a post, method overrides don't apply.
            if (!httpMethod.ValueEquals("POST"))
            {
                return httpMethod;
            }

            // Get the method override and if it's not for a GET or POST, then apply it.
            var methodOverride = request.SafeGet(r => r.Headers["X-HTTP-Method-Override"]) ??
                                 request.SafeGet(r => GetFormValue(r, "X-HTTP-Method-Override")) ??
                                 request.SafeGet(r => GetQueryStringValue(r, "X-HTTP-Method-Override"));

            if (methodOverride != null &&
                (!methodOverride.ValueEquals("GET") && !methodOverride.ValueEquals("POST")))
            {
                return methodOverride;
            }

            // Otherwise, just return the http method.
            return httpMethod;
        }

        private static string GetFormValue(HttpRequestBase request, string key)
        {
            return request.Unvalidated().QueryString[key] ?? request.Form[key];
        }

        private static string GetQueryStringValue(HttpRequestBase request, string key)
        {
            return request.Unvalidated().Form[key] ?? request.QueryString[key];
        }
    }
}