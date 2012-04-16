using System.Linq;
using System.Net.Http;

namespace AttributeRouting.Web.Http
{
    /// <summary>
    /// The route information for an action.
    /// </summary>
    public class HttpRouteAttribute : RouteAttributeBase
    {
        public HttpRouteAttribute(string routeUrl)
            : base(routeUrl)
        {
            
        }

        public HttpRouteAttribute(string routeUrl, params HttpMethod[] allowedMethods) 
            : base(routeUrl)
        {
            HttpMethods = allowedMethods.Select(m => m.Method).ToArray();
        }
    }
}