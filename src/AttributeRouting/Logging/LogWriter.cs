using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Logging
{
    public static class LogWriter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="writer"></param>
        public static void LogNumberOfRoutes(int count, TextWriter writer)
        {
            writer.WriteLine("TOTAL ROUTES: {0}", count);

            writer.WriteLine(new String('=', 40));
            writer.WriteLine(" ");
        }

        /// <summary>
        /// Logs an AttributeRouteInfo object to a TextWriter
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <param name="routeInfo"> </param>
        public static void LogRoute(TextWriter writer, string name, AttributeRouteInfo routeInfo)
        {
            writer.WriteLine("URL: {0} {1}", routeInfo.Url, routeInfo.HttpMethod);

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
