using System;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Constraints;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class StandardUsageSteps
    {
        [Then(@"the (?:(\d+)(?:st|nd|rd|th)\s)?route(?:'s)? url is ""(.*)""")]
        public void ThenTheRouteUrlIs(string nth, string url)
        {
            var i = nth.HasValue() ? int.Parse(nth) - 1 : 0;
            var routes = ScenarioContext.Current.GetFetchedRoutes();

            Assert.That(routes.Count(), Is.GreaterThan(i), "There is no {0} route available.", nth);

            var route = routes.ElementAt(i);

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Url, Is.EqualTo(url));
        }

        [Then(@"the default for ""(.*?)"" is ""(.*?)""")]
        public void ThenTheDefaultForIs(string key, object value)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().FirstOrDefault();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Defaults[key], Is.EqualTo(value));
        }

        [Then(@"a default for ""(.*?)"" does not exist")]
        public void ThenTheDefaultForDoesNotExist(string key)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().FirstOrDefault();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Defaults[key], Is.Null);
        }

        [Then(@"the route area is ""(.*?)""")]
        public void ThenTheRouteAreaIs(string area)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().FirstOrDefault();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.DataTokens["area"], Is.EqualTo(area));
        }

        [Then(@"the namespace is ""(.*?)""")]
        public void ThenTheNamespaceIs(string ns)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().FirstOrDefault();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.DataTokens["namespaces"], Is.EqualTo(new[] { ns }));
        }

        [Then(@"the route has a data token for ""(.*?)""")]
        public void ThenTheRouteHasADataTokeFor(string key)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().FirstOrDefault();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.DataTokens[key], Is.Not.Null);
        }

        [Then(@"the route is constrained to (.*?) requests")]
        public void ThenTheRouteIsConstrainedToRequests(string method)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().FirstOrDefault();

            AssertThatRouteIsConstrainedToHttpMethod(route, method);
        }

        [Then(@"the route for (.*?) is constrained to (.*?) requests")]
        public void ThenTheRouteForIsConstrainedToRequests(string action, string method)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().FirstOrDefault(r => r.Defaults["action"].ToString() == action);

            AssertThatRouteIsConstrainedToHttpMethod(route, method);
        }

        private void AssertThatRouteIsConstrainedToHttpMethod(Route route, string method)
        {
            var constraint = route.Constraints["inboundHttpMethod"] as IInboundHttpMethodConstraint;

            if (method.HasValue())
            {
                Assert.That(constraint, Is.Not.Null);
                Assert.That(constraint.AllowedMethods.Any(m => m.Equals(method, StringComparison.OrdinalIgnoreCase)), Is.True);
            }
            else
            {
                Assert.That(constraint, Is.Null);
            }            
        }
    }
}
