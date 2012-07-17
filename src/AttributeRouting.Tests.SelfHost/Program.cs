using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using AttributeRouting.Web.Http.SelfHost;
using AttributeRouting.Web.Http.SelfHost.Logging;

namespace AttributeRouting.Tests.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            // Attribute Routing
            config.Routes.MapHttpAttributeRoutes(cfg =>
            {
                cfg.ScanAssemblyOf<ProductsController>();

                // Must have this on, otherwise you need to specify RouteName in your attributes
                cfg.AutoGenerateRouteNames = true;
            });

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();

                Console.WriteLine("Routes:");

                config.Routes.Cast<HttpRoute>().LogTo(Console.Out);

                Console.WriteLine("Routes:");
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
