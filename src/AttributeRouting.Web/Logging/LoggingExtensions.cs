using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Logging;

namespace AttributeRouting.Web.Logging
{
    public static class LoggingExtensions
    {
        public static void LogTo(this IEnumerable<Route> routes, TextWriter writer)
        {
            var enumerable = routes as Route[] ?? routes.ToArray();
            
            LogWriter.LogNumberOfRoutes(enumerable.Count(), writer);

            foreach (var route in enumerable)
            {
                route.LogTo(writer);
            }
        }

        public static void LogTo(this Route route, TextWriter writer)
        {
            var name = route is IAttributeRoute ? ((IAttributeRoute)route).RouteName : null;

            LogWriter.LogRoute(writer, name, AttributeRouteInfo.GetRouteInfo(route.Url, route.Defaults, route.Constraints, route.DataTokens));
        }
    }
}