using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Routing;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs
{
    public static class ScenarioContextExtensions
    {
        public static void SetFetchedRoutes(this ScenarioContext context, IEnumerable<Route> routes)
        {
            context.Set(routes.ToList(), "FetchedRoutes");
        }

        public static IEnumerable<Route> GetFetchedRoutes(this ScenarioContext context)
        {
            return context.Get<IEnumerable<Route>>("FetchedRoutes");
        }

        public static void SetCurrentHttpContext(this ScenarioContext context, HttpContextBase httpContext)
        {
            context.Set(httpContext, "CurrentHttpContext");    
        }

        public static HttpContextBase GetCurrentHttpContext(this ScenarioContext context)
        {
            return context.Get<HttpContextBase>("CurrentHttpContext");    
        }

        public static IEnumerable<Route> GetFetchedWebRoutes(this ScenarioContext context) {
            return context.GetFetchedRoutes().OfType<Route>();
        }

        public static IEnumerable<HttpRoute> GetFetchedHttpSelfHostRoutes(this ScenarioContext context) {
            return context.GetFetchedRoutes().OfType<HttpRoute>();
        }
    }
}
