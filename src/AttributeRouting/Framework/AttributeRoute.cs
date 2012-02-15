using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Routing;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Route supporting the AttributeRouting framework.
    /// </summary>
    public class AttributeRoute : Route
    {
        private readonly AttributeRoutingConfiguration _configuration;

        /// <summary>
        /// Route supporting the AttributeRouting framework.
        /// </summary>
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

        /// <summary>
        /// The name of this route, for supporting named routes.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The translations available for this route.
        /// </summary>
        public IEnumerable<AttributeRoute> Translations { get; internal set; }

        /// <summary>
        /// The culture name associated with this route.
        /// </summary>
        public string CultureName { get; internal set; }

        /// <summary>
        /// List of all the subdomains mapped via AttributeRouting.
        /// </summary>
        public List<string> MappedSubdomains { get; set; }

        /// <summary>
        /// The subdomain this route is to be applied against.
        /// </summary>
        public string Subdomain { get; set; }

        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        {
            // If no subdomains are mapped with AR, then just resort to default behavior.
            if (!MappedSubdomains.Any())
                return base.GetRouteData(httpContext);

            // Get the subdomain from the requested hostname.
            var subdomain = _configuration.SubdomainParser(httpContext.Request.Headers["host"]);

            // Handle the request if this route is mapped to the requested host's subdomain
            if ((Subdomain ?? _configuration.DefaultSubdomain).ValueEquals(subdomain)) 
                return base.GetRouteData(httpContext);

            // Otherwise, this route does not match the request.
            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var virtualPathData = base.GetVirtualPath(requestContext, values);
            if (virtualPathData == null)
                return null;

            if (_configuration.TranslationProviders.Any())
                virtualPathData = GetTranslatedVirtualPath(virtualPathData, requestContext, values);

            var virtualPath = virtualPathData.VirtualPath;

            // NOTE: The initial lowercasing of all BUT url params occurs in RouteBuilder.CreateRouteUrl().
            // This is just a final lowercasing of the final, parameter-replaced url.
            if (_configuration.UseLowercaseRoutes && !_configuration.PreserveCaseForUrlParameters)
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

            if (path.HasValue() && !path.EndsWith("/"))
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