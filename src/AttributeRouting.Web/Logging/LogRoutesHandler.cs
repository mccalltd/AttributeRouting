using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
using AttributeRouting.Logging;

namespace AttributeRouting.Web.Logging
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

        private static string GetOutput(object tokenReplacements)
        {
            // Read the contents of the html template.
            var assembly = Assembly.GetExecutingAssembly();
            var fileName = "{0}.LogRoutes.html".FormatWith(typeof(LogRoutesHandler).Namespace);
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

        private static string GetRouteInfoOutput()
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

        private static void BuildCollectionOutput(StringBuilder builder, IDictionary<string, string> dictionary)
        {
            builder.Append("<td>");
            if (dictionary.Count == 0)
                builder.Append("&nbsp;");
            else
                foreach (var pair in dictionary)
                    builder.AppendFormat("<i>{0}</i>: {1}<br />", pair.Key, pair.Value);
            builder.Append("</td>");
        }

        private static IEnumerable<AttributeRouteInfo> GetRouteInfo()
        {
            return
                RouteTable.Routes.Cast<Route>().Select(
                    route =>
                    AttributeRouteInfo.GetRouteInfo(route.Url, route.Defaults, route.Constraints, route.DataTokens));
        }
    }
}