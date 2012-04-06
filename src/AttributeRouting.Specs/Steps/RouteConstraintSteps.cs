using System.Linq;
using AttributeRouting.Framework;
using AttributeRouting.Web;
using AttributeRouting.Web.Mvc.Framework;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class RouteConstraintSteps
    {
        [Then(@"the parameter ""(.*?)"" is constrained by the pattern ""(.*?)""")]
        public void ThenTheParameterIsContrainedBy(string key, object pattern)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Constraints[key], Is.TypeOf(typeof(RegexRouteConstraint)));
            Assert.That(((RegexRouteConstraint)route.Constraints[key]).Pattern, Is.EqualTo(pattern));
        }

        [Then(@"the route named ""(.*)"" has a constraint on ""(.*)"" of ""(.*)""")]
        public void ThenTheRouteNamedHasAConstraintOnOf(string routeName, string key, string value)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().Cast<AttributeRoute>().SingleOrDefault(r => r.Container.RouteName == routeName);

            Assert.That(route, Is.Not.Null);

            var constraint = route.Constraints[key];

            Assert.That(constraint, Is.Not.Null);
            Assert.That(constraint, Is.TypeOf(typeof(RegexRouteConstraint)));
            Assert.That(((RegexRouteConstraint)route.Constraints[key]).Pattern, Is.EqualTo(value));
        }
    }
}
