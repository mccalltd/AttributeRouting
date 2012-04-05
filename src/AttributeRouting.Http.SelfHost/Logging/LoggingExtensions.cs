using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Logging;

namespace AttributeRouting.Http.SelfHost.Logging
{
    public static class LoggingExtensions
    {
        public static void LogTo(this IEnumerable<HttpRoute> routes, TextWriter writer)
        {
            LogWriter.LogNumberOfRoutes(routes.Count(), writer);

            foreach (var route in routes)
                route.LogTo(writer);
        }

        public static void LogTo(this HttpRoute route, TextWriter writer)
        {
            string name = route is IAttributeRouteContainer 
                ? ((IAttributeRouteContainer)route).RouteName : null;

            LogWriter.LogRoute(writer, route.RouteTemplate, AttributeRouteInfo.GetRouteInfo(route.RouteTemplate, route.Defaults, route.Constraints, route.DataTokens));
        }
    }
}