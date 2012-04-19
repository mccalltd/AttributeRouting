using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public string RouteName { get; set; }

        public string CultureName { get; set; }

        public List<string> MappedSubdomains { get; set; }

        public string Subdomain { get; set; }

        public bool? UseLowercaseRoute { get; set; }

        public bool? PreserveCaseForUrlParameters { get; set; }

        public bool? AppendTrailingSlash { get; set; }

        IDictionary<string, object> IAttributeRoute.DataTokens
        {
            get { return DataTokens; }
            set { throw new NotImplementedException("HttpRoute.DataTokens has no setter."); }
        }

        IDictionary<string, object> IAttributeRoute.Constraints
        {
            get { return Constraints; }
            set { throw new NotImplementedException("HttpRoute.Constraints has no setter."); }
        }

        IDictionary<string, object> IAttributeRoute.Defaults
        {
            get { return Defaults; }
            set { throw new NotImplementedException("HttpRoute.Defaults has no setter."); }
        }

        public IEnumerable<IAttributeRoute> Translations { get; set; }

        public IAttributeRoute DefaultRouteContainer { get; set; }

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
            {
                virtualPathData = 
                    this.GetTranslatedVirtualPath(t => ((HttpRoute)t).GetVirtualPath(controllerContext, values))
                    ?? virtualPathData;
            }

            var virtualPath = this.GetFinalVirtualPath(virtualPathData.VirtualPath, _configuration);

            return new HttpVirtualPathData(virtualPathData.Route, virtualPath);
        }
    }
}
