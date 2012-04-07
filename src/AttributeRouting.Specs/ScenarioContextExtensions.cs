using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
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

        public static IEnumerable<Web.Mvc.Framework.AttributeRoute> GetFetchedMvcRoutes(this ScenarioContext context) {
            return context.GetFetchedRoutes().OfType<Web.Mvc.Framework.AttributeRoute>();
        }

        public static IEnumerable<Web.Http.WebHost.Framework.AttributeRoute> GetFetchedHttpWebHostRoutes(this ScenarioContext context) {
            return context.GetFetchedRoutes().OfType<Web.Http.WebHost.Framework.AttributeRoute>();
        }

        public static IEnumerable<IAttributeRouteContainer> GetFetchedRouteContainers(this ScenarioContext context) {
            IEnumerable<IAttributeRouteContainer> mvcRoutes = context.GetFetchedMvcRoutes().Select(r => r.Container);
            IEnumerable<IAttributeRouteContainer> webRoutes = context.GetFetchedHttpWebHostRoutes().Select(r => r.Container);

            return mvcRoutes.Union(webRoutes);
        }
    }
}
