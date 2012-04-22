using System.Linq;
using System.Web.Routing;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Specs.Subjects.Http;
using AttributeRouting.Web.Constraints;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Mvc;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class SharedSteps
    {
        [Given(@"I generate the routes defined in the subject controllers")]
        public void GivenIGenerateTheRoutesDefinedInTheSubjectControllers()
        {
            RouteTable.Routes.Clear();
            
            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.ScanAssemblyOf<StandardUsageController>();
                config.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
            });

            RouteTable.Routes.MapHttpAttributeRoutes(config =>
            {
                config.ScanAssemblyOf<HttpStandardUsageController>();
                config.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
            });
        }

        [When(@"I fetch the routes for the (.*?) controller's (.*?) action")]
        public void WhenIFetchTheRoutesFor(string controllerName, string actionName)
        {
            var routes = from route in RouteTable.Routes.Cast<Route>()
                         where route.Defaults["controller"].ToString() == controllerName &&
                               route.Defaults["action"].ToString() == actionName
                         select route;

            ScenarioContext.Current.SetFetchedRoutes(routes);
        }

        [When(@"I fetch the routes for the (.*?) controller")]
        public void WhenIFetchTheRoutesFor(string controllerName)
        {
            var routes = from route in RouteTable.Routes.Cast<Route>()
                         where route.Defaults["controller"].ToString() == controllerName
                         select route;

            ScenarioContext.Current.SetFetchedRoutes(routes);
        }

        [Then(@"(.*?) routes? are found")]
        public void ThenNRoutesShouldBeFound(int n)
        {
            var routes = ScenarioContext.Current.GetFetchedRoutes();

            Assert.That(routes.Count(), Is.EqualTo(n));
        }

        [Then(@"the data token for ""(.*)"" is ""(.*)""")]
        public void ThenTheDataTokenForKeyIsValue(string key, string value)
        {
            var route = ScenarioContext.Current.GetFetchedRoutes().First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.DataTokens.ContainsKey(key), Is.True);
            Assert.That(route.DataTokens[key], Is.EqualTo(value));
        }
    }
}
