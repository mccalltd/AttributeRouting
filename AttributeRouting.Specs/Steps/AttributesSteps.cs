using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class AttributesSteps
    {
        private IEnumerable<Route> _routes;

        [Given(@"I generate the routes defined in the subject controllers")]
        public void GivenIGenerateTheRoutesDefinedInTheSubjectControllers()
        {
            if (RouteTable.Routes.Count == 0)
                RouteTable.Routes.MapAttributeRoutes();
        }

        [When(@"I fetch the routes for the (.*?) controller's (.*?) action")]
        public void WhenIFetchTheRoutesFor(string controllerName, string actionName)
        {
            _routes = from route in RouteTable.Routes.Cast<Route>()
                      where route.Defaults["controller"].ToString() == controllerName &&
                            route.Defaults["action"].ToString() == actionName
                      select route;
        }

        [When(@"I fetch the routes for the (.*?) controller")]
        public void WhenIFetchTheRoutesFor(string controllerName)
        {
            _routes = from route in RouteTable.Routes.Cast<Route>()
                      where route.Defaults["controller"].ToString() == controllerName
                      select route;
        }

        [Then(@"(.*?) routes? should be found")]
        public void ThenNRoutesShouldBeFound(int n)
        {
            Assert.That(_routes.Count(), Is.EqualTo(n));
        }

        [Then(@"the (?:(\d+)(?:st|nd|rd|th)\s)?route url is ""(.*)""")]
        public void ThenTheRouteUrlIs(string nth, string url)
        {
            var i = nth.HasValue() ? int.Parse(nth) - 1 : 0;
            var route = _routes.ElementAt(i);

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Url, Is.EqualTo(url));
        }

        [Then(@"the parameter (.*?) is optional")]
        public void ThenTheParameterIsOptional(string name)
        {
            var route = _routes.First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Defaults[name], Is.EqualTo(UrlParameter.Optional));
        }

        [Then(@"the default for ""(.*?)"" is ""(.*?)""")]
        public void ThenTheDefaultForIs(string key, object value)
        {
            var route = _routes.First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Defaults[key], Is.EqualTo(value));
        }

        [Then(@"the parameter ""(.*?)"" is constrained by the pattern ""(.*?)""")]
        public void ThenTheParameterIsContrainedBy(string key, object pattern)
        {
            var route = _routes.First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Constraints[key], Is.TypeOf(typeof(RegexRouteConstraint)));
            Assert.That(((RegexRouteConstraint)route.Constraints[key]).Pattern, Is.EqualTo(pattern));
        }

        [Then(@"the namespace is ""(.*?)""")]
        public void ThenTheNamespaceIs(string ns)
        {
            var route = _routes.First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.DataTokens["namespaces"], Is.EqualTo(new[] { ns }));
        }

        [Then(@"the route for (.*?) is constrained to (.*?) requests")]
        public void ThenTheRouteForIsConstrainedToRequests(string action, string method)
        {
            var route = _routes.SingleOrDefault(r => r.Defaults["action"].ToString() == action);

            Assert.That(route, Is.Not.Null);

            var constraint = route.Constraints["httpMethod"] as RestfulHttpMethodConstraint;

            Assert.That(constraint, Is.Not.Null);
            Assert.That(constraint.AllowedMethods.Count, Is.EqualTo(1));
            Assert.That(constraint.AllowedMethods.First(), Is.EqualTo(method));
        }

        [Then(@"the route named ""(.*)"" has a default for ""(.*)"" of (.*)")]
        public void ThenTheRouteNamedHasADefaultForOf(string routeName, string key, string value)
        {
            var route = _routes.Cast<AttributeRoute>().SingleOrDefault(r => r.Name == routeName);

            Assert.That(route, Is.Not.Null);

            var routeDefault = route.Defaults[key];
            
            Assert.That(routeDefault, Is.Not.Null);
            Assert.That(routeDefault.ToString(), Is.EqualTo(value));
        }

        [Then(@"the route named ""(.*)"" has a constraint on ""(.*)"" of ""(.*)""")]
        public void ThenTheRouteNamedHasAConstraintOnOf(string routeName, string key, string value)
        {
            var route = _routes.Cast<AttributeRoute>().SingleOrDefault(r => r.Name == routeName);

            Assert.That(route, Is.Not.Null);

            var constraint = route.Constraints[key];

            Assert.That(constraint, Is.Not.Null);
            Assert.That(constraint, Is.TypeOf(typeof(RegexRouteConstraint)));
            Assert.That(((RegexRouteConstraint)route.Constraints[key]).Pattern, Is.EqualTo(value));
        }

        [Then(@"the data token for ""(.*)"" is ""(.*)""")]
        public void ThenTheDataTokenForAreaIsArea(string key, string value)
        {
            var route = _routes.First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.DataTokens.ContainsKey(key), Is.True);
            Assert.That(route.DataTokens[key], Is.EqualTo(value));
        }
    }
}
