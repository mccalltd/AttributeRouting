using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
using AttributeRouting.Logging;

namespace AttributeRouting.Mvc.Logging
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

            LogWriter.LogRoute(writer, route.Url, name, route.Defaults, route.Constraints, route.DataTokens);
        }
    }
}