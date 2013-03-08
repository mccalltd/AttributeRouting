using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using AttributeRouting.Framework;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class RouteDefaultsSteps
    {
        [Then(@"the parameter ""(.*?)"" is optional")]
        public void ThenTheParameterIsOptional(string name)
        {
            var routes = ScenarioContext.Current.GetFetchedRoutes();

            foreach (var route in routes) {
                Assert.That(route, Is.Not.Null);
                Assert.That(route.Defaults[name], Is.EqualTo(UrlParameter.Optional).Or.EqualTo(RouteParameter.Optional));
            }
        }

        [Then(@"the route named ""(.*)"" has a default for ""(.*)"" of ""?(.*?)""?")]
        public void ThenTheRouteNamedHasADefaultForOf(string routeName, string key, string value)
        {
            var routes = ScenarioContext.Current.GetFetchedRoutes()
                .Cast<IAttributeRoute>()
                .Where(c => c.RouteName == routeName);

            foreach (var route in routes) {
                Assert.That(route, Is.Not.Null);

                var routeDefault = route.Defaults[key];

                Assert.That(routeDefault, Is.Not.Null);
                Assert.That(routeDefault.ToString(), Is.EqualTo(value));
            }
        }
    }
}
