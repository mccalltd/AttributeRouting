using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting
{
    public class LogRoutesHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var writer = context.Response.Output;

            var output = GetOutput(new { items = GetRouteInfoOutput() });

            writer.Write(output);
        }

        private string GetOutput(object tokenReplacements)
        {
            // Read the contents of the html template.
            var assembly = Assembly.GetExecutingAssembly();
            var fileName = "{0}.LogRoutes.html".FormatWith(assembly.GetName().Name);
            string fileContent;
            using (var stream = assembly.GetManifestResourceStream(fileName))
            {
                if (stream == null)
                    throw new AttributeRoutingException(
                        "The file \"{0}\" cannot be found as an embedded resource.".FormatWith(fileName));

                using (var reader = new StreamReader(stream))
                    fileContent = reader.ReadToEnd();
            }

            // Replace tokens in the template with appropriate content
            var outputBuilder = new StringBuilder(fileContent);

            var tokenReplacementsDictionary = new RouteValueDictionary(tokenReplacements);
            foreach (var key in tokenReplacementsDictionary.Keys)
                outputBuilder.Replace("{{{0}}}".FormatWith(key), tokenReplacementsDictionary[key].ToString());

            return outputBuilder.ToString();
        }

        private string GetRouteInfoOutput()
        {
            var outputBuilder = new StringBuilder();

            var routeInfo = GetRouteInfo();
            var row = 0;
            foreach (var info in routeInfo)
            {
                outputBuilder.AppendFormat("<tr class=\"{0}\">", (++row % 2 == 0) ? "even" : "odd");
                outputBuilder.AppendFormat("<td>{0}</td>", info.HttpMethod);
                outputBuilder.AppendFormat("<td>{0}</td>", info.Url);
                outputBuilder.AppendFormat("<td>{0}</td>", info.Endpoint);
                outputBuilder.AppendFormat("<td>{0}</td>", info.Area);
                outputBuilder.AppendFormat("<td>{0}</td>", info.Namespaces);
                outputBuilder.Append("</tr>");
            }

            return outputBuilder.ToString();
        }

        private IEnumerable<RouteInfo> GetRouteInfo()
        {
            var items = new List<RouteInfo>();

            foreach (var route in RouteTable.Routes.Cast<Route>())
            {
                var item = new RouteInfo { Url = route.Url };

                if (route.Defaults != null)
                {
                    object controller;
                    if (route.Defaults.TryGetValue("controller", out controller))
                        item.Controller = controller.ToString();

                    object action;
                    if (route.Defaults.TryGetValue("action", out action))
                        item.Action = action.ToString();
                }

                if (route.Constraints != null)
                {
                    object httpMethod;
                    if (route.Constraints.TryGetValue("httpMethod", out httpMethod))
                        item.HttpMethod = ((RestfulHttpMethodConstraint)httpMethod).AllowedMethods.Aggregate((n1, n2) => n1 + ", " + n2);
                }

                if (route.DataTokens != null)
                {
                    object area;
                    if (route.DataTokens.TryGetValue("area", out area))
                        item.Area = area.ToString();

                    object namespaces;
                    if (route.DataTokens.TryGetValue("namespaces", out namespaces))
                        item.Namespaces = ((string[])namespaces).Aggregate((n1, n2) => n1 + ", " + n2);
                }

                items.Add(item);
            }

            return items;
        }

        private class RouteInfo
        {
            public string Url { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public string HttpMethod { get; set; }
            public string Area { get; set; }
            public string Namespaces { get; set; }

            public string Endpoint
            {
                get
                {
                    if (!Action.HasValue())
                        return Controller;

                    return "{0}#{1}".FormatWith(Controller, Action);
                }
            }
        }
    }
}
