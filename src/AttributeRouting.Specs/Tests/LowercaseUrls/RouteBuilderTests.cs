using System;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Logging;
using AttributeRouting.Specs.Subjects;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.LowercaseUrls
{
    public class RouteBuilderTests
    {
        [Test]
        public void It_builds_routes_with_all_but_url_params_lowercased_when_so_configured()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c =>
            {
                c.AddRoutesFromController<LowercaseUrlController>();
                c.UseLowercaseRoutes = true;
            });

            routes.Cast<AttributeRoute>().LogTo(Console.Out);

            var route = routes.Cast<AttributeRoute>().FirstOrDefault();
            Assert.That(route, Is.Not.Null);
            Assert.That(route.Url, Is.EqualTo("lowercaseurl/hello/{userName}/goodbye"));
        }

        [Test]
        public void It_builds_routes_with_urls_in_specified_case_when_not_configured_to_generate_lowercase_routes()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c => c.AddRoutesFromController<LowercaseUrlController>());

            routes.Cast<AttributeRoute>().LogTo(Console.Out);

            var route = routes.Cast<AttributeRoute>().FirstOrDefault();
            Assert.That(route, Is.Not.Null);
            Assert.That(route.Url, Is.EqualTo("LowercaseUrl/Hello/{userName}/Goodbye"));
        }
    }
}
