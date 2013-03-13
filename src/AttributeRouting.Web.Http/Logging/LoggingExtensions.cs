using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
using AttributeRouting.Logging;

namespace AttributeRouting.Web.Http.Logging
{
    public static class LoggingExtensions
    {
        public static void LogTo(HttpRouteCollection routes, TextWriter writer)
        {
            LogWriter.LogNumberOfRoutes(routes.Count(), writer);

            foreach (var route in routes)
            {
                route.LogTo(writer);
            }
        }

        public static void LogTo(this IHttpRoute route, TextWriter writer)
        {
            var attributeRoute = route as IAttributeRoute;
            var info = RouteLoggingInfo.GetRouteInfo(route.RouteTemplate,
                                                     route.Defaults,
                                                     attributeRoute.SafeGet(r => r.QueryStringDefaults),
                                                     route.Constraints,
                                                     attributeRoute.SafeGet(r => r.QueryStringConstraints),
                                                     route.DataTokens);

            LogWriter.LogRoute(writer, route.RouteTemplate, info);
        }
    }
}