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
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <param name="name"></param>
        public static void LogRoute(TextWriter writer, string url, string name,
            IDictionary<string, object> defaults, 
            IDictionary<string, object> constraints,
            IDictionary<string, object> dataTokens)
        {
            writer.WriteLine("URL: {0}", url);

            if (name != null)
                writer.WriteLine("NAME: {0}", name);

            if (defaults != null && defaults.Count > 0)
            {
                writer.WriteLine("DEFAULTS:");
                foreach (var key in defaults.Keys)
                    writer.WriteLine("- {0} = {1}", key, defaults[key]);
            }

            if (constraints != null && constraints.Count > 0)
            {
                writer.WriteLine("CONSTRAINTS:");
                foreach (var key in constraints.Keys)
                {
                    object value;
                    if (constraints[key].GetType() == typeof(IRestfulHttpMethodConstraint))
                        value = ((IRestfulHttpMethodConstraint)constraints[key]).AllowedMethods.First();
                    else if (constraints[key].GetType() == typeof(IRegexRouteConstraint))
                        value = ((IRegexRouteConstraint)constraints[key]).Pattern;
                    else
                        value = constraints[key];

                    writer.WriteLine("- {0} = {1}", key, value);
                }
            }

            if (dataTokens != null && dataTokens.Count > 0)
            {
                writer.WriteLine("DATA TOKENS:");
                foreach (var key in dataTokens.Keys)
                {
                    if (key.ValueEquals("namespaces"))
                        writer.WriteLine("- {0} = {1}", key,
                                         ((string[])dataTokens[key]).Aggregate((n1, n2) => n1 + ", " + n2));
                    else
                        writer.WriteLine("- {0} = {1}", key, dataTokens[key]);
                }
            }

            writer.WriteLine(" ");
            writer.WriteLine(new String('-', 40));
            writer.WriteLine(" ");
        }
    }
}
