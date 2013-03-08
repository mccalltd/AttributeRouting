using System.Linq;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Web.Constraints;
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
                if (constraint == null && route is IAttributeRoute)
                {
                    constraint = ((IAttributeRoute)route).QueryStringConstraints[key];
                }
                Assert.That(constraint, Is.Not.Null);

                // If this is a querystring route constraint wrapper, then unwrap it.
                var queryStringConstraint = constraint as IQueryStringRouteConstraint;
                if (queryStringConstraint != null && queryStringConstraint.Constraint != null)
                {
                    constraint = queryStringConstraint.Constraint;
                }

                var compoundRouteConstraint = constraint as ICompoundRouteConstraint;
                if (compoundRouteConstraint != null)
                {
                    Assert.That(compoundRouteConstraint.Constraints.Any(c => c.GetType().FullName == type), Is.True);
                }
                else
                {
                    Assert.That(constraint.GetType().FullName, Is.EqualTo(type));
                }
            }
        }

        [Then(@"the route named ""(.*)"" has a constraint on ""(.*)"" of ""(.*)""")]
        public void ThenTheRouteNamedHasAConstraintOnOf(string routeName, string key, string value) 
        {
            var routes = ScenarioContext.Current.GetFetchedRoutes()
                .Cast<IAttributeRoute>()
                .Where(r => r.RouteName == routeName);

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
