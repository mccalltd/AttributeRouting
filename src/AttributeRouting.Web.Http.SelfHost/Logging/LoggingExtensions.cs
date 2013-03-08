using System.IO;
using System.Linq;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
using AttributeRouting.Logging;

namespace AttributeRouting.Web.Http.SelfHost.Logging
{
    public static class LoggingExtensions
    {
        public static void LogTo(this HttpRoute[] routes, TextWriter writer)
        {
            LogWriter.LogNumberOfRoutes(routes.Count(), writer);

            foreach (var route in routes)
            {
                route.LogTo(writer);
            }
        }

        public static void LogTo(this HttpRoute route, TextWriter writer)
        {
            var attributeRoute = route as IAttributeRoute;
            var info = RouteLoggingInfo.GetRouteInfo(route.RouteTemplate,
                                                     route.Defaults,
                                                     route.Constraints,
                                                     attributeRoute.SafeGet(x => x.QueryStringConstraints),
                                                     route.DataTokens);

            LogWriter.LogRoute(writer, route.RouteTemplate, info);
        }
    }
}