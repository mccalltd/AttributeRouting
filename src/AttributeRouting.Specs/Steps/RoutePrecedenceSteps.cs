using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Mvc;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class RoutePrecedenceSteps
    {
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
    }
}
