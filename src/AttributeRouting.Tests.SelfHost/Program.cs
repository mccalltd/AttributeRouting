using System;
using System.Linq;
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
            var config = new HttpSelfHostConfiguration("http://localhost:8081");

            // Attribute Routing
            config.Routes.MapHttpAttributeRoutes();

            using (var server = new HttpSelfHostServer(config))
            {
                // Be sure to run as an admin!
                server.OpenAsync().Wait();

                Console.WriteLine("Routes:");

                config.Routes.Cast<HttpRoute>().ToArray().LogTo(Console.Out);

                Console.WriteLine("Routes:");
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
