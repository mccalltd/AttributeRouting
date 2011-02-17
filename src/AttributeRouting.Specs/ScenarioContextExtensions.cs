using System.Collections.Generic;
using System.Web.Routing;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs
{
    public static class ScenarioContextExtensions
    {
        public static void SetFetchedRoutes(this ScenarioContext context, IEnumerable<Route> routes)
        {
            context.Set(routes, "FetchedRoutes");
        }

        public static IEnumerable<Route> GetFetchedRoutes(this ScenarioContext context)
        {
            return context.Get<IEnumerable<Route>>("FetchedRoutes");
        }
    }
}
