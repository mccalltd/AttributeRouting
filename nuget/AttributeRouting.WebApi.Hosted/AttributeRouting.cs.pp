using System.Reflection;
using System.Web.Http.SelfHost;
using AttributeRouting.Web.Http.SelfHost;

namespace $rootnamespace$ {
    public static class AttributeRouting {

		// Call this static method from a start up class in your applicaton (e.g.Program.cs)
		// Pass in the configuration you're using for your self-hosted Web API
		public static void RegisterRoutes(HttpSelfHostConfiguration config) {
            
			// See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			// To debug routes locally, you can log to Console.Out (or any other TextWriter) like so:
			//     config.Routes.Cast<HttpRoute>().LogTo(Console.Out);

			// Self-hosted Web API

            // Attribute Routing
            config.Routes.MapHttpAttributeRoutes(cfg =>
            {
                cfg.ScanAssembly(Assembly.GetExecutingAssembly());

                // Must have this on, otherwise you need to specify RouteName in your attributes
                cfg.AutoGenerateRouteNames = true;
            });
		}
    }
}