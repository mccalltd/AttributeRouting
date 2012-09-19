using System.Linq;
using System.Web.Routing;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class RoutePrecedenceSteps
    {
        [Then(@"the routes from the (.*) controller precede those from the (.*) controller")]
        public void ThenTheRoutesFromTheFirstControllerPrecedeThoseFromTheNextController(string firstControllerName, string secondControllerName)
        {
            var routes = RouteTable.Routes.Cast<Route>();

            var anyRouteFromFirstController = routes.Any(r => r.Defaults["controller"].ToString() == firstControllerName);

            Assert.That(anyRouteFromFirstController, Is.True);

            var secondControllerRange = routes.SkipWhile(r => r.Defaults["controller"].ToString() != secondControllerName);
            var firstControllerRouteInRange = secondControllerRange.Any(r => r.Defaults["controller"].ToString() == firstControllerName);
            
            Assert.That(firstControllerRouteInRange, Is.False);
        }

        [Then(@"no routes follow the routes from the (.*) controller")]
        public void ThenNoRoutesFollowTheRoutesFromTheController(string controllerName)
        {
            var count = 0;
            var indexOfLastRouteForController = 0;
            var routes = RouteTable.Routes.Cast<Route>();

            foreach (var route in routes.ToList())
            {
                var routeControllerName = route.Defaults["controller"].ToString();
                var skipControllerNames = new[]
                {
                    "RoutePrecedenceAmongTheSitesRoutes",
                    "RoutePrecedenceViaRouteProperties"
                };

                if (skipControllerNames.Any(routeControllerName.EndsWith))
                {
                    // Skip the controllers that will come after controllers registered through the config object
                    // due to have actions that specify a SitePrecedence property.
                    continue;
                }

                if (routeControllerName == controllerName)
                    indexOfLastRouteForController = count;

                count++;
            }

            Assert.That(indexOfLastRouteForController, Is.EqualTo(count - 1));
        }
    }
}
