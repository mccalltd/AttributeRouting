using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs
{
    public static class ScenarioContextExtensions
    {
        public static void SetFetchedRoutes(this ScenarioContext context, IEnumerable<Route> routes)
        {
            context.Set(routes.ToList(), "FetchedRoutes");
        }

        public static IEnumerable<IAttributeRoute> GetFetchedRoutes(this ScenarioContext context)
        {
            var routes = context.Get<IEnumerable<Route>>("FetchedRoutes");

            return routes.OfType<IAttributeRoute>();
        }

        public static IEnumerable<Route> GetFetchedWebRoutes(this ScenarioContext context) {
            return context.GetFetchedRoutes().OfType<Route>();
        }

        public static IEnumerable<HttpRoute> GetFetchedHttpSelfHostRoutes(this ScenarioContext context) {
            return context.GetFetchedRoutes().OfType<HttpRoute>();
        }
    }
}
