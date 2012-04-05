using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Logging;

namespace AttributeRouting.AspNet.Logging
{
    public static class LoggingExtensions
    {
        public static void LogTo(this IEnumerable<Route> routes, TextWriter writer)
        {
            LogWriter.LogNumberOfRoutes(routes.Count(), writer);

            foreach (var route in routes)
                route.LogTo(writer);
        }

        public static void LogTo(this Route route, TextWriter writer)
        {
            string name = route is IAttributeRouteContainer 
                ? ((IAttributeRouteContainer)route).RouteName : null;

            LogWriter.LogRoute(writer, name, AttributeRouteInfo.GetRouteInfo(route.Url, route.Defaults, route.Constraints, route.DataTokens));
        }
    }
}