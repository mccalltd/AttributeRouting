using System;
using System.Web.Routing;
using AttributeRouting.Web.Logging;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs.Steps
{
    [Binding]
    public class LoggingDefinitions
    {
        [When(@"I log the routes")]
        public void WhenILogTheRoutes()
        {
            RouteTable.Routes.LogTo(Console.Out);            
        }

        [Then(@"ta-da!")]
        public void Then()
        {
            
        }
    }
}
