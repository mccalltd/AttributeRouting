using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Mvc;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests
{
    public class RouteBuilderTests
    {
        [Test]
        [ExpectedException(typeof(AttributeRoutingException),
                           ExpectedMessage = "You must specify an assembly or controller to scan for routes.")]
        public void It_will_throw_an_exception_if_no_controller_types_are_registered_via_configuration()
        {
            // Arrange
            var routes = RouteTable.Routes;
            routes.Clear();
            var config = new Configuration();

            // Act
            routes.MapAttributeRoutes(config);
        }
    }
}
