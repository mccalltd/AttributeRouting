using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Logging
{
    public static class LoggingExtensions
    {
        public static void LogTo(this IEnumerable<Route> routes, TextWriter writer)
        {
            writer.WriteLine("TOTAL ROUTES: {0}", routes.Count());

            writer.WriteLine(new String('=', 40));
            writer.WriteLine(" ");

            foreach (var route in routes)
                route.LogTo(writer);
        }

        public static void LogTo(this Route route, TextWriter writer)
        {
            writer.WriteLine("URL: {0}", route.Url);

            if (route is IAttributeRouteContainer)
                writer.WriteLine("NAME: {0}", ((IAttributeRouteContainer)route).RouteName);

            if (route.Defaults != null && route.Defaults.Count > 0)
            {
                writer.WriteLine("DEFAULTS:");
                foreach (var key in route.Defaults.Keys)
                    writer.WriteLine("- {0} = {1}", key, route.Defaults[key]);
            }

            if (route.Constraints != null && route.Constraints.Count > 0)
            {
                writer.WriteLine("CONSTRAINTS:");
                foreach (var key in route.Constraints.Keys)
                {
                    object value;
                    if (route.Constraints[key].GetType() == typeof(IRestfulHttpMethodConstraint))
                        value = ((IRestfulHttpMethodConstraint)route.Constraints[key]).AllowedMethods.First();
                    else if (route.Constraints[key].GetType() == typeof(IRegexRouteConstraint))
                        value = ((IRegexRouteConstraint)route.Constraints[key]).Pattern;
                    else
                        value = route.Constraints[key];

                    writer.WriteLine("- {0} = {1}", key, value);
                }
            }

            if (route.DataTokens != null && route.DataTokens.Count > 0)
            {
                writer.WriteLine("DATA TOKENS:");
                foreach (var key in route.DataTokens.Keys)
                {
                    if (key.ValueEquals("namespaces"))
                        writer.WriteLine("- {0} = {1}", key,
                                         ((string[])route.DataTokens[key]).Aggregate((n1, n2) => n1 + ", " + n2));
                    else
                        writer.WriteLine("- {0} = {1}", key, route.DataTokens[key]);
                }
            }

            writer.WriteLine(" ");
            writer.WriteLine(new String('-', 40));
            writer.WriteLine(" ");
        }
    }
}