using System.Linq;
using AttributeRouting.Web.Constraints;
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
            var routes = ScenarioContext.Current.GetFetchedRoutes();

            foreach (var route in routes)
            {
                Assert.That(route, Is.Not.Null);
                Assert.That(route.Constraints[key], Is.TypeOf(typeof(RegexRouteConstraint)));
                Assert.That(((RegexRouteConstraint)route.Constraints[key]).Pattern, Is.EqualTo(pattern));
            }
        }

        [Then(@"the parameter ""(.*?)"" is constrained by an inline (.*)")]
        public void ThenTheParameterIsConstrainedByAnInline(string key, string type)
        {
            var routes = ScenarioContext.Current.GetFetchedRoutes();

            foreach (var route in routes)
            {
                Assert.That(route, Is.Not.Null);

                var constraint = route.Constraints[key];

                Assert.That(constraint, Is.Not.Null);
                Assert.That(constraint.GetType().FullName, Is.EqualTo(type));
            }
        }

        [Then(@"the route named ""(.*)"" has a constraint on ""(.*)"" of ""(.*)""")]
        public void ThenTheRouteNamedHasAConstraintOnOf(string routeName, string key, string value) {
            var routes = ScenarioContext.Current.GetFetchedRoutes().Where(r => r.RouteName == routeName);

            foreach (var route in routes)
            {
                Assert.That(route, Is.Not.Null);

                var constraint = route.Constraints[key];

                Assert.That(constraint, Is.Not.Null);
                Assert.That(constraint, Is.TypeOf(typeof(RegexRouteConstraint)));
                Assert.That(((RegexRouteConstraint)route.Constraints[key]).Pattern, Is.EqualTo(value));
            }
        }
    }
}
