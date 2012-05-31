using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Helpers;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Specs.Subjects.Http;
using AttributeRouting.Specs.Tests;
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

            RouteTable.Routes.MapAttributeRoutes(x =>
            {
                x.ScanAssemblyOf<StandardUsageController>();
                x.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                x.InheritActionsFromBaseController = true;
            });

            RouteTable.Routes.MapHttpAttributeRoutes(x =>
            {
                x.ScanAssemblyOf<HttpStandardUsageController>();
                x.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                x.InheritActionsFromBaseController = true;
            });
        }

        [Given(@"I have registered the routes for the (.*)")]
        public void GivenIHaveRegisteredTheRoutesForThe(string controllerName)
        {
            RouteTable.Routes.Clear();

            var type = typeof(StandardUsageController).Assembly.GetTypes().FirstOrDefault(t => t.Name == controllerName);

            RouteTable.Routes.MapAttributeRoutes(x =>
            {
                x.AddRoutesFromController(type);
                x.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                x.InheritActionsFromBaseController = true;
            });

            RouteTable.Routes.MapHttpAttributeRoutes(x =>
            {
                x.AddRoutesFromController(type);
                x.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                x.InheritActionsFromBaseController = true;
            });
        }

        [When(@"I fetch the routes for the (.*?) controller's (.*?) action")]
        [When(@"I fetch the routes for the (.*?)Controller's (.*?) action")]
        public void WhenIFetchTheRoutesFor(string controllerName, string actionName)
        {
            var routes = from route in RouteTable.Routes.Cast<Route>()
                         where route.Defaults["controller"].ToString() == controllerName &&
                               route.Defaults["action"].ToString() == actionName
                         select route;

            ScenarioContext.Current.SetFetchedRoutes(routes);
        }

        [When(@"I fetch the routes for the (.*?) controller")]
        [When(@"I fetch the routes for the (.*?)Controller")]
        public void WhenIFetchTheRoutesFor(string controllerName)
        {
            var routes = from route in RouteTable.Routes.Cast<Route>()
                         where route.Defaults["controller"].ToString() == controllerName
                         select route;

            ScenarioContext.Current.SetFetchedRoutes(routes);
        }

        [When(@"an?(\s.*)? request for ""(.*)"" is made")]
        public void WhenAMethodRequestForUrlIsMade(string method, string url)
        {
            var desiredMethod = (method.HasValue() ? method : "GET").Trim().ToUpperInvariant();
            var requestMethod = (desiredMethod.ValueEquals("GET") ? "GET" : "POST").ToUpperInvariant();

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns(url);
                r.SetupGet(x => x.HttpMethod).Returns(requestMethod);
               
                if (desiredMethod != requestMethod)
                {
                    r.SetupGet(x => x.Headers).Returns(new NameValueCollection
                    {
                        { "X-HTTP-Method-Override", desiredMethod }
                    });

                    r.SetupGet(x => x.HttpMethod).Returns(desiredMethod);
                }
            });

            ScenarioContext.Current.SetCurrentHttpContext(httpContextMock.Object);
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

        [Then(@"the (.*) action is( not)? matched")]
        public void ThenTheActionIsMatched(string actionName, string isNot)
        {
            var routeForAction = (from route in RouteTable.Routes.Cast<Route>()
                                  where route.Defaults["action"].ToString() == actionName
                                  select route).FirstOrDefault();

            Assert.That(routeForAction, Is.Not.Null);

            var currentHttpContext = ScenarioContext.Current.GetCurrentHttpContext();
            var routeData = routeForAction.GetRouteData(currentHttpContext);

            Assert.That(routeData, isNot.HasValue() ? Is.Null : Is.Not.Null);
        }
    }
}
