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
                outputBuilder.AppendFormat("<td class=\"url\">{0}</td>", info.Url);

                BuildCollectionOutput(outputBuilder, info.Defaults);
                BuildCollectionOutput(outputBuilder, info.Constraints);
                BuildCollectionOutput(outputBuilder, info.DataTokens);
                
                outputBuilder.Append("</tr>");
            }

            return outputBuilder.ToString();
        }

        private void BuildCollectionOutput(StringBuilder builder, IDictionary<string, string> dictionary)
        {
            builder.Append("<td>");
            if (dictionary.Count == 0)
                builder.Append("&nbsp;");
            else
                foreach (var pair in dictionary)
                    builder.AppendFormat("<i>{0}</i>: {1}<br />", pair.Key, pair.Value);
            builder.Append("</td>");
        }

        private IEnumerable<RouteInfo> GetRouteInfo()
        {
            var items = new List<RouteInfo>();

            foreach (var route in RouteTable.Routes.Cast<Route>())
            {
                var item = new RouteInfo { Url = route.Url };

                if (route.Defaults != null)
                {
                    foreach (var @default in route.Defaults)
                    {
                        /*if (@default.Key.ValueEquals("controller"))
                            item.Controller = @default.Value.ToString();
                        else if (@default.Key.ValueEquals("action"))
                            item.Action = @default.Value.ToString();
                        else*/
                            item.Defaults.Add(@default.Key, @default.Value.ToString());
                    }
                }

                if (route.Constraints != null)
                {
                    foreach (var constraint in route.Constraints)
                    {
                        if (constraint.Value.GetType() == typeof(RestfulHttpMethodConstraint))
                            item.HttpMethod = ((RestfulHttpMethodConstraint)constraint.Value).AllowedMethods.Aggregate((n1, n2) => n1 + ", " + n2);
                        else if (constraint.Value.GetType() == typeof(RegexRouteConstraint))
                            item.Constraints.Add(constraint.Key, ((RegexRouteConstraint)constraint.Value).Pattern);
                        else
                            item.Constraints.Add(constraint.Key, constraint.Value.ToString());
                    }
                }

                if (route.DataTokens != null)
                {
                    foreach (var token in route.DataTokens)
                    {
                        if (token.Key.ValueEquals("namespaces"))
                            item.DataTokens.Add(token.Key, ((string[])token.Value).Aggregate((n1, n2) => n1 + ", " + n2));
                        else
                            item.DataTokens.Add(token.Key, token.Value.ToString());
                    }
                }

                items.Add(item);
            }

            return items;
        }

        private class RouteInfo
        {
            public RouteInfo()
            {
                Defaults = new Dictionary<string, string>();
                Constraints = new Dictionary<string, string>();
                DataTokens = new Dictionary<string, string>();
            }

            public string Url { get; set; }
            public string HttpMethod { get; set; }
            public IDictionary<string, string> Defaults { get; set; }
            public IDictionary<string, string> Constraints { get; set; }
            public IDictionary<string, string> DataTokens { get; set; }
        }
    }
}
