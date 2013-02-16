using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Mvc;
using AttributeRouting.Web.Mvc.Framework;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Subdomains
{
    public class RouteBuilderTests
    {
        [Test]
        public void Adds_subdomain_to_attribute_route_when_specified_in_area_attribute()
        {
            var routes = RouteTable.Routes;
            routes.Clear();

            var configuration = new Configuration();
            configuration.AddRoutesFromController<SubdomainController>();
            routes.MapAttributeRoutes(configuration);

            var route = routes.Single() as IAttributeRoute;
            Assert.That(route, Is.Not.Null);
            Assert.That(route.Subdomain == "users");
            Assert.That(configuration.MappedSubdomains.Count, Is.EqualTo(1));
            Assert.That(configuration.MappedSubdomains.Single(), Is.EqualTo("users"));
        }
    }
}
