using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Routing;

namespace AttributeRouting.Framework
{
    public class AttributeRoute : Route
    {
        private readonly AttributeRoutingConfiguration _configuration;

        public AttributeRoute(
            string url,
            RouteValueDictionary defaults,
            RouteValueDictionary constraints,
            RouteValueDictionary dataTokens,
            AttributeRoutingConfiguration configuration)
            : base(url, defaults, constraints, dataTokens, configuration.RouteHandlerFactory())
        {
            _configuration = configuration;
        }

        public string Name { get; internal set; }

        public IEnumerable<AttributeRoute> Translations { get; internal set; }

        public string CultureName { get; internal set; }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var virtualPathData = base.GetVirtualPath(requestContext, values);
            if (virtualPathData == null)
                return null;

            if (_configuration.TranslationProvider != null)
                virtualPathData = GetTranslatedVirtualPath(virtualPathData, requestContext, values);

            var virtualPath = virtualPathData.VirtualPath;

            if (_configuration.UseLowercaseRoutes)
                virtualPath = TransformVirtualPathToLowercase(virtualPath);

            if (_configuration.AppendTrailingSlash)
                virtualPath = AppendTrailingSlashToVirtualPath(virtualPath);

            virtualPathData.VirtualPath = virtualPath;

            return virtualPathData;
        }

        private VirtualPathData GetTranslatedVirtualPath(VirtualPathData virtualPathData, RequestContext requestContext, RouteValueDictionary values)
        {
            if (Translations == null || !Translations.Any())
                return virtualPathData;

            var currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            // Try and get the language-culture translation, then fall back to language translation
            var translation = Translations.FirstOrDefault(t => t.CultureName == currentCultureName)
                              ?? Translations.FirstOrDefault(t => currentCultureName.StartsWith(t.CultureName));

            if (translation == null)
                return virtualPathData;

            return translation.GetVirtualPath(requestContext, values);
        }

        private static string TransformVirtualPathToLowercase(string virtualPath)
        {
            string path, query;
            GetPathAndQuery(virtualPath, out path, out query);

            return path.ToLowerInvariant() + query;
        }

        private static string AppendTrailingSlashToVirtualPath(string virtualPath)
        {
            string path, query;
            GetPathAndQuery(virtualPath, out path, out query);

            if (!path.EndsWith("/"))
                path += "/";

            return path + query;
        }

        private static void GetPathAndQuery(string virtualPath, out string path, out string query)
        {
            // NOTE: Do not lowercase the querystring vals
            var match = Regex.Match(virtualPath, @"(?<path>[^\?]*)(?<query>\?.*)?");

            // Just covering my backside here in case the regex fails for some reason.
            if (!match.Success)
            {
                path = virtualPath;
                query = null;
            }
            else
            {
                path = match.Groups["path"].Value;
                query = match.Groups["query"].Value;                            
            }
        }
    }
}