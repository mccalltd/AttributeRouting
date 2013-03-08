using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.Script.Serialization;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
using AttributeRouting.Logging;

namespace AttributeRouting.Web.Logging
{
    public class LogRoutesHandler : IHttpHandler
    {
        private readonly Lazy<string> _scriptSources = new Lazy<string>(LoadScriptSources);

        private static string LoadScriptSources()
        {
            var scriptsBuilder = new StringBuilder();
            var scriptSources = new[]
            {
                "AttributeRouting.Web.Logging.jquery-1.9.1.min.js",
                "AttributeRouting.Web.Logging.knockout-2.2.1.js"
            };

            var assembly = Assembly.GetExecutingAssembly();
            foreach (var source in scriptSources)
            {
                using (var stream = assembly.GetManifestResourceStream(source))
                {
                    if (stream == null) continue;

                    using (var reader = new StreamReader(stream))
                    {
                        scriptsBuilder.AppendFormat("<script>{0}</script>", reader.ReadToEnd());
                    }
                }
            }

            return scriptsBuilder.ToString();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var writer = context.Response.Output;

            var output = GetOutput();

            writer.Write(output);
        }

        private string GetOutput()
        {
            // Read the contents of the html template.
            var assembly = Assembly.GetExecutingAssembly();
            var fileName = "{0}.LogRoutes.html".FormatWith(typeof(LogRoutesHandler).Namespace);
            string fileContent;
            using (var stream = assembly.GetManifestResourceStream(fileName))
            {
                if (stream == null)
                {
                    throw new AttributeRoutingException(
                        "The file \"{0}\" cannot be found as an embedded resource.".FormatWith(fileName));
                }

                using (var reader = new StreamReader(stream))
                {
                    fileContent = reader.ReadToEnd();
                }
            }

            // Prepare to build the output.
            var outputBuilder = new StringBuilder(fileContent);

            // Send in the raw script sources to eliminate reliance on CDN.
            outputBuilder.Replace("{scripts}", _scriptSources.Value);

            // Send in the json data for the routes.
            var model = new JavaScriptSerializer().Serialize(new
            {
                routes = GetRouteInfo()
            });
            outputBuilder.Replace("\"{data}\"", model);

            return outputBuilder.ToString();
        }

        private static IEnumerable<object> GetRouteInfo()
        {
            return from r in RouteTable.Routes.OfType<Route>()
                   let ar = r as IAttributeRoute
                   let routeInfo = RouteLoggingInfo.GetRouteInfo(r.Url,
                                                                 r.Defaults,
                                                                 r.Constraints,
                                                                 ar.SafeGet(x => x.QueryStringConstraints),
                                                                 r.DataTokens)
                   select new
                   {
                       methods = routeInfo.HttpMethods,
                       url = routeInfo.Url,
                       defaults = routeInfo.Defaults.Select(kvp => new { key = kvp.Key, value = kvp.Value }),
                       constraints = routeInfo.Constraints.Select(kvp => new { key = kvp.Key, value = kvp.Value }),
                       dataTokens = routeInfo.DataTokens.Select(kvp => new { key = kvp.Key, value = kvp.Value })
                   };
        }
    }
}