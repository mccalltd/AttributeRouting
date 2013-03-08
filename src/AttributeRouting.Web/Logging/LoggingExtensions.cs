using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
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
            var attributeRoute = route as IAttributeRoute;
            var name = attributeRoute.SafeGet(r => r.RouteName);
            var info = RouteLoggingInfo.GetRouteInfo(route.Url,
                                                     route.Defaults,
                                                     route.Constraints,
                                                     attributeRoute.SafeGet(r => r.QueryStringConstraints),
                                                     route.DataTokens);

            LogWriter.LogRoute(writer, name, info);
        }
    }
}