using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttributeRouting.Tests.Web
{
    public class CultureAwareHandler
    {
        private const string CultureRouteParamName = "culture";

        public static void CheckRequestContext(RequestContext requestContext)
        {
            // Detect the current culture from route data first 
            // (will be in here if url {culture} is part of the route).
            var currentCultureName = (string)requestContext.RouteData.Values[CultureRouteParamName];
            if (currentCultureName == null)
            {
                // Fallback to detecting the user language.
                var request = requestContext.HttpContext.Request;
                if (request.UserLanguages != null && request.UserLanguages.Any())
                    currentCultureName = request.UserLanguages[0];
            }

            if (currentCultureName != null)
            {
                // Set the current thread's culture based on the detected culture.
                var cultureInfo = new CultureInfo(currentCultureName);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                // Add the culture to route data
                requestContext.RouteData.Values[CultureRouteParamName] = currentCultureName;
            }

            
        }
    }
    public class CultureAwareRouteHandler : MvcRouteHandler
    {
        protected override System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            CultureAwareHandler.CheckRequestContext(requestContext);

            return base.GetHttpHandler(requestContext);
        }
    }

    public class HttpCultureAwareRoutingHandler : HttpControllerRouteHandler
    {
        protected override System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            CultureAwareHandler.CheckRequestContext(requestContext);

            return base.GetHttpHandler(requestContext);
        }
    }
}