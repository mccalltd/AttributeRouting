using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using AttributeRouting.Web.Logging;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class LoggingDefinitions
    {
        [When(@"I log the routes")]
        public void WhenILogTheRoutes()
        {
            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);            
        }

        [Then(@"ta-da!")]
        public void Then()
        {
            Assert.Pass();
        }
    }
}
