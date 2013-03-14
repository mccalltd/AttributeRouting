using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using AttributeRouting.Helpers;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Specs.Subjects.Http;
using AttributeRouting.Specs.Tests;
using AttributeRouting.Web.Http.Constraints;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Mvc;
using AttributeRouting.Web.Mvc.Constraints;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class SharedSteps
    {
        private RouteConfiguration _configuration;
        private HttpWebRouteConfiguration _httpConfiguration;

        private RouteConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new RouteConfiguration();
                    _configuration.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                    _configuration.InlineRouteConstraints.Add("colorValue", typeof(EnumValueRouteConstraint<Color>));
                }
                return _configuration;
            }
            set { _configuration = value; }
        }

        private HttpWebRouteConfiguration HttpRouteConfiguration
        {
            get
            {
                if (_httpConfiguration == null)
                {
                    _httpConfiguration = new HttpWebRouteConfiguration();
                    _httpConfiguration.InlineRouteConstraints.Add("color", typeof(EnumHttpRouteConstraint<Color>));
                    _httpConfiguration.InlineRouteConstraints.Add("colorValue", typeof(EnumValueHttpRouteConstraint<Color>));
                }
                return _httpConfiguration;
            }
            set { _httpConfiguration = value; }
        }

        [Given(@"I generate the routes defined in the subject controllers")]
        public void GivenIGenerateTheRoutesDefinedInTheSubjectControllers()
        {
            RouteTable.Routes.Clear();

            RouteTable.Routes.MapAttributeRoutes(x =>
            {
                x.AddRoutesFromAssemblyOf<StandardUsageController>();
                x.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                x.InlineRouteConstraints.Add("colorValue", typeof(EnumValueRouteConstraint<Color>));
                x.InheritActionsFromBaseController = true;
            });

            GlobalConfiguration.Configuration.Routes.MapHttpAttributeRoutes(x =>
            {
                x.AddRoutesFromAssemblyOf<HttpStandardUsageController>();
                x.InlineRouteConstraints.Add("color", typeof(EnumHttpRouteConstraint<Color>));
                x.InlineRouteConstraints.Add("colorValue", typeof(EnumValueHttpRouteConstraint<Color>));
                x.InheritActionsFromBaseController = true;
            });
        }

        [Given(@"I have registered the routes for the (.*)")]
        public void GivenIHaveRegisteredTheRoutesForThe(string controllerName)
        {
            RouteTable.Routes.Clear();

            var type = typeof(StandardUsageController).Assembly.GetTypes().FirstOrDefault(t => t.Name == controllerName);

            if (controllerName.Contains("Http"))
            {
                GlobalConfiguration.Configuration.Routes.MapHttpAttributeRoutes(x =>
                {
                    x.AddRoutesFromController(type);
                    x.InlineRouteConstraints.Add("color", typeof(EnumHttpRouteConstraint<Color>));
                    x.InlineRouteConstraints.Add("colorValue", typeof(EnumValueHttpRouteConstraint<Color>));
                    x.InheritActionsFromBaseController = true;
                });                
            }
            else
            {
                RouteTable.Routes.MapAttributeRoutes(x =>
                {
                    x.AddRoutesFromController(type);
                    x.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                    x.InlineRouteConstraints.Add("colorValue", typeof(EnumValueRouteConstraint<Color>));
                    x.InheritActionsFromBaseController = true;
                });
            }
        }

        [Given(@"I have a new configuration object")]
        public void GivenIHaveANewConfigurationObject()
        {
            Configuration = null;
            HttpRouteConfiguration = null;
        }

        [Given(@"I add the routes from the (.*) controller")]
        public void GivenIAddTheRoutesFromTheController(string controllerName)
        {
            var controllerType = GetControllerType(controllerName);
            
            if (controllerName.StartsWith("Http"))
            {
                HttpRouteConfiguration.AddRoutesFromController(controllerType);
            }
            else
            {
                Configuration.AddRoutesFromController(controllerType);
            }
        }

        [Given(@"I add the routes from controllers derived from the (.*) controller")]
        public void GivenIAddTheRoutesFromControllersOfTypeBaseController(string baseControllerName)
        {
            var baseControllerType = GetControllerType(baseControllerName);
            
            if (baseControllerName.StartsWith("Http"))
            {
                HttpRouteConfiguration.AddRoutesFromControllersOfType(baseControllerType);
            }
            else
            {
                Configuration.AddRoutesFromControllersOfType(baseControllerType);
            }
        }

        [Given(@"I add the (.*) routes from the executing assembly")]
        public void GivenIAddTheRoutesFromTheExecutingAssembly(string type)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            if (type.ValueEquals("Mvc"))
            {
                Configuration.AddRoutesFromAssembly(executingAssembly);
            }
            else
            {
                HttpRouteConfiguration.AddRoutesFromAssembly(executingAssembly);
            }
        }

        [Given(@"I generate the routes with this configuration")]
        [When(@"I generate the routes with this configuration")]
        public void WhenIGenerateTheRoutesWithThisConfiguration()
        {
            RouteTable.Routes.Clear();

            if (_configuration != null)
            {
                RouteTable.Routes.MapAttributeRoutes(Configuration);
            }

            if (_httpConfiguration != null)
            {
                GlobalConfiguration.Configuration.Routes.MapHttpAttributeRoutes(HttpRouteConfiguration);
            }
        }

        [When(@"I fetch the routes for the (.*?) controller's (.*?) action")]
        [When(@"I fetch the routes for the (.*?)Controller's (.*?) action")]
        public void WhenIFetchTheRoutesFor(string controllerName, string actionName)
        {
            var routes = (from route in RouteTable.Routes.Cast<Route>()
                          where route.Defaults["controller"].ToString() == controllerName &&
                                route.Defaults["action"].ToString() == actionName
                          select route).ToList();

            foreach (var route in routes)
            {
                Console.WriteLine(route.Url);
            }

            ScenarioContext.Current.SetFetchedRoutes(routes);
        }

        [When(@"I fetch all the routes")]
        public void WhenIFetchAllTheRoutes()
        {
            var routes = RouteTable.Routes.Cast<Route>();
            
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
            var pathAndQuery = url.SplitAndTrim("?");

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.HttpMethod).Returns(requestMethod);
                r.SetupGet(x => x.AppRelativeCurrentExecutionFilePath).Returns("~/" + Regex.Replace(pathAndQuery[0], @"[{}]", ""));
                
                if (pathAndQuery.Length > 1)
                {
                    r.SetupGet(x => x.QueryString).Returns(HttpUtility.ParseQueryString(pathAndQuery[1]));
                }
               
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

        private Type GetControllerType(string controllerName)
        {
            if (controllerName.StartsWith("Http"))
                controllerName = "Http." + controllerName;

            var typeName = String.Format("AttributeRouting.Specs.Subjects.{0}Controller, AttributeRouting.Specs",
                                         controllerName);

            var type = Type.GetType(typeName);

            Assert.That(type, Is.Not.Null, "The controller type \"{0}\" could not be resolved.", typeName);

            return type;
        }
    }
}
