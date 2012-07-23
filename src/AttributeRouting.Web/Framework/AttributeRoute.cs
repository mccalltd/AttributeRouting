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
        private readonly WebAttributeRoutingConfigurationBase _configuration;

        /// <summary>
        /// Route used by the AttributeRouting framework in web projects.
        /// </summary>
        public AttributeRoute(string url,
                              RouteValueDictionary defaults,
                              RouteValueDictionary constraints,
                              RouteValueDictionary dataTokens,
                              WebAttributeRoutingConfigurationBase configuration)
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

        public SemanticVersion MinVersion { get; set; }

        public SemanticVersion MaxVersion { get; set; }

        /// <summary>
        /// Shortcut to set <see cref="MinVersion"/> with a string
        /// </summary>
        public string MinVer
        {
            get { return MinVersion.ToString(); }
            set { MinVersion = SemanticVersion.Parse(value, allowNull: true); }
        }

        /// <summary>
        /// Shortcut to set <see cref="MaxVersion"/> with a string
        /// </summary>
        public string MaxVer
        {
            get { return MaxVersion.ToString(); }
            set { MaxVersion = SemanticVersion.Parse(value, allowNull: true); }
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Optimize matching by comparing the static left part of the route url with the requested path.
            var requestedPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
            if (!this.IsLeftPartOfUrlMatched(requestedPath))
                return null;

            // Let the underlying route match, and if it does, then add a few more constraints.
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null)
                return null;

            // Constrain by subdomain if configured
            var host = httpContext.SafeGet(ctx => ctx.Request.Headers["host"]);
            if (!this.IsSubdomainMatched(host, _configuration))
                return null;

            // Constrain by culture name if configured
            var currentUICultureName = _configuration.CurrentUICultureResolver(httpContext, routeData);
            if (!this.IsCultureNameMatched(currentUICultureName, _configuration))
                return null;

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Let the underlying route do its thing, and if it does, then add some functionality on top.
            var virtualPathData = base.GetVirtualPath(requestContext, values);
            if (virtualPathData == null)
                return null;

            // Translate this path if a translation is available.
            if (_configuration.TranslationProviders.Any())
            {
                virtualPathData = 
                    this.GetTranslatedVirtualPath(t => ((Route)t).GetVirtualPath(requestContext, values))
                    ?? virtualPathData;
            }

            // Lowercase, append trailing slash, etc.
            var virtualPath = this.GetFinalVirtualPath(virtualPathData.VirtualPath, _configuration);
            virtualPathData.VirtualPath = virtualPath;

            return virtualPathData;
        }
    }
}
