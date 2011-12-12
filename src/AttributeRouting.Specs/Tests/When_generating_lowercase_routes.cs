using System.Web.Routing;
using AttributeRouting.Framework;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests
{
    public class When_generating_lowercase_routes
    {
        [Test]
        public void Then_all_route_values_are_lowercased()
        {
            var routeValues = new RouteValueDictionary
            {
                { "area", "Dominion" }, 
                { "controller", "Lord" }, 
                { "action", "Rule" }, 
                { "whom", "Peasants" }
            };

            var attributeRoute = new AttributeRoute(null, null, null, null, null, true, false);

            var values = attributeRoute.GetLowercaseRouteValues(routeValues);
            
            Assert.That(values["area"], Is.EqualTo("dominion"));
            Assert.That(values["controller"], Is.EqualTo("lord"));
            Assert.That(values["action"], Is.EqualTo("rule"));
            Assert.That(values["whom"], Is.EqualTo("peasants"));
        }
    }
}
