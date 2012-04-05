using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using AttributeRouting.Mvc;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class RoutePrecedenceSteps
    {
        private AttributeRoutingConfiguration _configuration;

        [Given(@"I have a new configuration object")]
        public void GivenIHaveANewConfigurationObject()
        {
            _configuration = new AttributeRoutingConfiguration();
        }

        [Given(@"I add the routes from the (.*) controller")]
        public void GivenIAddTheRoutesFromTheController(string controllerName)
        {
            var controllerType = GetControllerType(controllerName);
            _configuration.AddRoutesFromController(controllerType);
        }

        [Given(@"I add the routes from controllers derived from the (.*) controller")]
        public void GivenIAddTheRoutesFromControllersOfTypeBaseController(string baseControllerName)
        {
            var baseControllerType = GetControllerType(baseControllerName);
            _configuration.AddRoutesFromControllersOfType(baseControllerType);
        }

        [When(@"I generate the routes with this configuration")]
        public void WhenIGenerateTheRoutesWithThisConfiguration()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(_configuration);
        }

        [Then(@"the routes from the (.*) controller precede those from the (.*) controller")]
        public void ThenTheRoutesFromTheFirstControllerPrecedeThoseFromTheNextController(string firstControllerName, string secondControllerName)
        {
            var routes = RouteTable.Routes.Cast<Route>();

            var anyRouteFromFirstController = routes.Any(r => r.Defaults["controller"].ToString() == firstControllerName);

            Assert.That(anyRouteFromFirstController, Is.True);

            var secondControllerRange = routes.SkipWhile(r => r.Defaults["controller"].ToString() != secondControllerName);
            var firstControllerRouteInRange = secondControllerRange.Any(r => r.Defaults["controller"].ToString() == firstControllerName);
            
            Assert.That(firstControllerRouteInRange, Is.False);
        }

        private Type GetControllerType(string controllerName)
        {
            var typeName = String.Format("AttributeRouting.Specs.Subjects.{0}Controller, AttributeRouting.Specs",
                                         controllerName);

            var type = Type.GetType(typeName);

            Assert.That(type, Is.Not.Null, "The controller type \"{0}\" could not be resolved.", typeName);

            return type;
        }
    }
}
