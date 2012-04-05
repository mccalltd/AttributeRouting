using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Logging;
using AttributeRouting.Mvc.Framework;
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
            RouteTable.Routes.Cast<AttributeRoute>().LogTo(Console.Out);
        }

        [Then(@"ta-da!")]
        public void Then()
        {
            Assert.Pass();
        }
    }
}
