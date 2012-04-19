using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Framework
{
    public class AttributeRoute : Route, IAttributeRoute
    {
        private readonly WebAttributeRoutingConfiguration _configuration;

        /// <summary>
        /// Route supporting the AttributeRouting framework.
        /// </summary>
        public AttributeRoute(string url,
                              RouteValueDictionary defaults,
                              RouteValueDictionary constraints,
                              RouteValueDictionary dataTokens,
                              WebAttributeRoutingConfiguration configuration)
            : base(url, defaults, constraints, dataTokens, configuration.RouteHandlerFactory())
        {
            _configuration = configuration;
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
            set { DataTokens = new RouteValueDictionary(value); }
        }

        IDictionary<string, object> IAttributeRoute.Constraints
        {
            get { return Constraints; }
            set { Constraints = new RouteValueDictionary(value); }
        }

        IDictionary<string, object> IAttributeRoute.Defaults
        {
            get { return Defaults; }
            set { Defaults = new RouteValueDictionary(value); }
        }

        public IEnumerable<IAttributeRoute> Translations { get; set; }

        public IAttributeRoute DefaultRouteContainer { get; set; }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null)
                return null;

            if (!IsSubdomainMatched(httpContext))
                return null;

            if (!IsCultureNameMatched(httpContext, routeData))
                return null;

            return routeData;
        }

        private bool IsSubdomainMatched(HttpContextBase httpContext)
        {
            // If no subdomains are mapped with AR, then yes.
            if (!MappedSubdomains.Any())
                return true;

            // Get the subdomain from the requested hostname.
            var subdomain = _configuration.SubdomainParser(httpContext.Request.Headers["host"]);

            // Match if this route is mapped to the requested host's subdomain
            if ((Subdomain ?? _configuration.DefaultSubdomain).ValueEquals(subdomain))
                return true;

            // Otherwise, this route does not match the request.
            return false;
        }

        private bool IsCultureNameMatched(HttpContextBase httpContext, RouteData routeData)
        {
            if (!_configuration.ConstrainTranslatedRoutesByCurrentUICulture)
                return true;

            // If no translations are available, then obviously the answer is yes.
            if (!_configuration.TranslationProviders.Any())
                return true;

            var currentUICultureName = _configuration.CurrentUICultureResolver(httpContext, routeData);
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

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var virtualPathData = base.GetVirtualPath(requestContext, values);
            if (virtualPathData == null)
                return null;

            if (_configuration.TranslationProviders.Any())
            {
                virtualPathData = 
                    this.GetTranslatedVirtualPath(t => ((Route)t).GetVirtualPath(requestContext, values))
                    ?? virtualPathData;
            }

            var virtualPath = this.GetFinalVirtualPath(virtualPathData.VirtualPath, _configuration);

            virtualPathData.VirtualPath = virtualPath;

            return virtualPathData;
        }
    }
}
