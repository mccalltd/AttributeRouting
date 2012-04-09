using System.Web.Routing;
using System.Web.Http;
using System.Web.Http.Routing;
using AttributeRouting.Web.Http.WebHost;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.AttributeRouting), "Start")]

namespace $rootnamespace$.App_Start {
    public static class AttributeRoutingHttp {
		public static void RegisterRoutes(RouteCollection routes) {
            // See http://github.com/mccalltd/AttributeRouting/wiki/3.-Configuration for more options.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd

			// ASP.NET Web API
            routes.MapHttpAttributeRoutes();
		}

        public static void Start() {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
