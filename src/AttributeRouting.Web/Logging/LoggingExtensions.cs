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
        public static void LogTo(this RouteCollection routes, TextWriter writer)
        {
            LogWriter.LogNumberOfRoutes(routes.Count(), writer);

            foreach (var route in routes.Cast<Route>())
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
                                                     attributeRoute.SafeGet(r => r.QueryStringDefaults),
                                                     route.Constraints,
                                                     attributeRoute.SafeGet(r => r.QueryStringConstraints),
                                                     route.DataTokens);

            LogWriter.LogRoute(writer, name, info);
        }
    }
}