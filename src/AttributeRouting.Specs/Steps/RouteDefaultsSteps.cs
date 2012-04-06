using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Framework;
using AttributeRouting.Web.Mvc.Framework;
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
            var route = ScenarioContext.Current.GetFetchedRoutes().First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Defaults[name], Is.EqualTo(UrlParameter.Optional));
        }

        [Then(@"the route named ""(.*)"" has a default for ""(.*)"" of ""?(.*?)""?")]
        public void ThenTheRouteNamedHasADefaultForOf(string routeName, string key, string value)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().Cast<AttributeRoute>().SingleOrDefault(r => r.Container.RouteName == routeName);

            Assert.That(route, Is.Not.Null);

            var routeDefault = route.Defaults[key];

            Assert.That(routeDefault, Is.Not.Null);
            Assert.That(routeDefault.ToString(), Is.EqualTo(value));
        }
    }
}
