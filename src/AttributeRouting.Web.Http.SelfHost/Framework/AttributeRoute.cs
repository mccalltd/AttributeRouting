using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.SelfHost.Framework
{
    public class AttributeRoute : HttpRoute, IAttributeRoute
    {
        private readonly HttpAttributeRoutingConfiguration _configuration;

        /// <summary>
        /// Route supporting the AttributeRouting framework.
        /// </summary>
        public AttributeRoute(string url,
                              HttpRouteValueDictionary defaults,
                              HttpRouteValueDictionary constraints,
                              HttpRouteValueDictionary dataTokens,
                              HttpAttributeRoutingConfiguration configuration)
            : base(url, defaults, constraints, dataTokens)
        {
            _configuration = configuration;
        }

        public string Url
        {
            get { return RouteTemplate; }
            set { throw new NotImplementedException("IHttpRoute.RouteTemplate has no setter."); }
        }

        public override IHttpRouteData GetRouteData(string virtualPathRoot, HttpRequestMessage request)
        {
            var routeData = base.GetRouteData(virtualPathRoot, request);
            if (routeData == null)
                return null;

            if (!IsSubdomainMatched(request))
                return null;

            if (!IsCultureNameMatched(request, routeData))
                return null;

            return routeData;
        }

        private bool IsSubdomainMatched(HttpRequestMessage request)
        {
            // If no subdomains are mapped with AR, then yes.
            if (!MappedSubdomains.Any())
                return true;

            // Get the subdomain from the requested hostname.
            var subdomain = _configuration.SubdomainParser(request.Headers.Host);

            // Match if this route is mapped to the requested host's subdomain
            if ((Subdomain ?? _configuration.DefaultSubdomain).ValueEquals(subdomain))
                return true;

            // Otherwise, this route does not match the request.
            return false;
        }

        private bool IsCultureNameMatched(HttpRequestMessage request, IHttpRouteData routeData)
        {
            if (!_configuration.ConstrainTranslatedRoutesByCurrentUICulture)
                return true;

            // If no translations are available, then obviously the answer is yes.
            if (!_configuration.TranslationProviders.Any())
                return true;

            var currentUICultureName = _configuration.CurrentUICultureResolver(request, routeData);
            var currentUINeutralCultureName = currentUICultureName.Split('-').First();

            // If this is a translated route:
            if (DefaultRouteContainer != null)
            {
                // Match if the current UI culture matches the culture name of this route.
                if (currentUICultureName.ValueEquals(CultureName))
                    return true;

                // Match if the culture name is neutral and no translation exists for the specific culture.
                if (CultureName.Split('-').Length == 1
                    && currentUINeutralCultureName == CultureName
                    && !DefaultRouteContainer.Translations.Any(t => t.CultureName.ValueEquals(currentUICultureName)))
                {
                    return true;
                }
            }
            else
            {
                // If this is a default route:

                // Match if this route has no translations.
                if (!Translations.Any())
                    return true;

                // Match if this route has no translations for the neutral current UI culture.
                if (!Translations.Any(t => t.CultureName == currentUINeutralCultureName))
                    return true;
            }

            // Otherwise, don't match.
            return false;
        }

        public override IHttpVirtualPathData GetVirtualPath(HttpControllerContext controllerContext,
                                                            IDictionary<string, object> values)
        {
            var virtualPathData = base.GetVirtualPath(controllerContext, values);
            if (virtualPathData == null)
                return null;

            if (_configuration.TranslationProviders.Any())
                virtualPathData = GetTranslatedVirtualPath(virtualPathData, controllerContext, values);

            var virtualPath = virtualPathData.VirtualPath;

            // NOTE: The initial lowercasing of all BUT url params occurs in RouteBuilder.CreateRouteUrl().
            // This is just a final lowercasing of the final, parameter-replaced url.
            if (_configuration.UseLowercaseRoutes && !_configuration.PreserveCaseForUrlParameters)
                virtualPath = TransformVirtualPathToLowercase(virtualPath);

            if (_configuration.AppendTrailingSlash)
                virtualPath = AppendTrailingSlashToVirtualPath(virtualPath);

            return new HttpVirtualPathData(virtualPathData.Route, virtualPath);
        }

        private IHttpVirtualPathData GetTranslatedVirtualPath(IHttpVirtualPathData virtualPathData,
                                                              HttpControllerContext requestContext,
                                                              IDictionary<string, object> values)
        {
            if (Translations == null || !Translations.Any())
                return virtualPathData;

            var currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            // Try and get the language-culture translation, then fall back to language translation
            var translation = Translations.FirstOrDefault(t => t.CultureName == currentCultureName)
                              ?? Translations.FirstOrDefault(t => currentCultureName.StartsWith(t.CultureName));

            if (translation == null)
                return virtualPathData;

            return ((HttpRoute)translation).GetVirtualPath(requestContext, values);
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

        /// <summary>
        /// The name of this route, for supporting named routes.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// The culture name associated with this route.
        /// </summary>
        public string CultureName { get; set; }

        /// <summary>
        /// List of all the subdomains mapped via AttributeRouting.
        /// </summary>
        public List<string> MappedSubdomains { get; set; }

        /// <summary>
        /// The subdomain this route is to be applied against.
        /// </summary>
        public string Subdomain { get; set; }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        IDictionary<string, object> IAttributeRoute.DataTokens
        {
            get { return DataTokens; }
            set { throw new NotImplementedException("HttpRoute.DataTokens has no setter."); }
        }

        /// <summary>
        /// Constraints dictionary
        /// </summary>
        IDictionary<string, object> IAttributeRoute.Constraints
        {
            get { return Constraints; }
            set { throw new NotImplementedException("HttpRoute.Constraints has no setter."); }
        }

        /// <summary>
        /// Defaults dictionary
        /// </summary>
        IDictionary<string, object> IAttributeRoute.Defaults
        {
            get { return Defaults; }
            set { throw new NotImplementedException("HttpRoute.Defaults has no setter."); }
        }

        /// <summary>
        /// The translations available for this route.
        /// </summary>
        public IEnumerable<IAttributeRoute> Translations { get; set; }

        /// <summary>
        /// Default route container back-reference
        /// </summary>
        public IAttributeRoute DefaultRouteContainer { get; set; }
    }
}
