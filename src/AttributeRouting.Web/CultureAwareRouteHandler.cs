using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttributeRouting.Web
{
    public class CultureAwareRouteHandler : MvcRouteHandler
    {
        private const string CultureRouteParamName = "culture";

        protected override System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // Detect the current culture.
            var cultureRouteData = requestContext.RouteData.Values[CultureRouteParamName];
            var currentCultureName = (string)cultureRouteData;
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
                requestContext.RouteData.Values[CultureRouteParamName] 
                    = currentCultureName;
            }

            return base.GetHttpHandler(requestContext);
        }
    }
}