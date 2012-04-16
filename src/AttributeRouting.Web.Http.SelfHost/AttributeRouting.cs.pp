using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using AttributeRouting.Web.Http.SelfHost;
using AttributeRouting.Web.Http.SelfHost.Logging;

namespace $rootnamespace$ {
    public static class AttributeRouting {

		// Call this static method from a start up class in your applicaton (e.g.Program.cs)
		// Pass in the configuration you're using for your self-hosted Web API
		public static void RegisterRoutes(HttpSelfHostConfiguration config) {
            // See http://github.com/mccalltd/AttributeRouting/wiki/3.-Configuration for more options.
			// To debug routes locally, you can use
			//
			//     config.Routes.Cast<HttpRoute>().LogTo(Console.Out);
			//
			// In a console application (or to any other TextWriter)

			// Self-hosted Web API

            // Attribute Routing
            config.Routes.MapHttpAttributeRoutes(cfg =>
            {
                cfg.ScanAssemblyOf<AttributeRouting>();

                // Must have this on, otherwise you need to specify RouteName
                // in your attributes
                cfg.AutoGenerateRouteNames = true;
            });
		}
    }
}