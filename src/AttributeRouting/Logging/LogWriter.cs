using System;
using System.IO;

namespace AttributeRouting.Logging
{
    public static class LogWriter
    {
        public static void LogNumberOfRoutes(int count, TextWriter writer)
        {
            writer.WriteLine("TOTAL ROUTES: {0}", count);

            writer.WriteLine(new String('=', 40));
            writer.WriteLine(" ");
        }

        public static void LogRoute(TextWriter writer, string name, RouteLoggingInfo routeInfo)
        {
            writer.WriteLine("URL: {0} {1}", routeInfo.Url, routeInfo.HttpMethods);

            if (name != null)
                writer.WriteLine("NAME: {0}", name);

            if (routeInfo.Defaults != null && routeInfo.Defaults.Count > 0)
            {
                writer.WriteLine("DEFAULTS:");
                foreach (var @default in routeInfo.Defaults)
                    writer.WriteLine("- {0} = {1}", @default.Key, @default.Value);
            }

            if (routeInfo.Constraints != null && routeInfo.Constraints.Count > 0)
            {
                writer.WriteLine("CONSTRAINTS:");
                foreach (var constraint in routeInfo.Constraints)                
                    writer.WriteLine("- {0} = {1}", constraint.Key, constraint.Value);                
            }

            if (routeInfo.DataTokens != null && routeInfo.DataTokens.Count > 0)
            {
                writer.WriteLine("DATA TOKENS:");
                foreach (var t in routeInfo.DataTokens)                
                    writer.WriteLine("- {0} = {1}", t.Key, t.Value);                
            }

            writer.WriteLine(" ");
            writer.WriteLine(new String('-', 40));
            writer.WriteLine(" ");
        }
    }
}
