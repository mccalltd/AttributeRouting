using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.Framework
{
    /// <summary>
    /// Route to use for self-hosted Web API routes.
    /// </summary>
    public class HttpAttributeRoute : HttpRoute, IAttributeRoute
    {
        private readonly HttpConfigurationBase _configuration;

        /// <summary>
        /// Route used by the AttributeRouting framework in self-host projects.
        /// </summary>
        public HttpAttributeRoute(string url,
                                  HttpRouteValueDictionary defaults,
                                  HttpRouteValueDictionary constraints,
                                  HttpRouteValueDictionary dataTokens,
                                  HttpConfigurationBase configuration)
            : base(url, defaults, constraints, dataTokens, configuration.MessageHandler)
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
            // Let the underlying route match, and if it does, then add a few more constraints.
            var routeData = base.GetRouteData(virtualPathRoot, request);
            if (routeData == null)
                return null;

            // Constrain by subdomain if configured
            var host = request.SafeGet(r => r.Headers.Host);
            if (!this.IsSubdomainMatched(host, _configuration))
                return null;

            // Constrain by culture name if configured
            var currentUICultureName = _configuration.CurrentUICultureResolver(request, routeData); 
            if (!this.IsCultureNameMatched(currentUICultureName, _configuration))
                return null;

            return routeData;
        }

        public override IHttpVirtualPathData GetVirtualPath(HttpRequestMessage request, IDictionary<string, object> values)
        {
            // Let the underlying route do its thing, and if it does, then add some functionality on top.
            var virtualPathData = this.GetVirtualPath(() => base.GetVirtualPath(request, values));
            if (virtualPathData == null)
                return null;

            // Translate this path if a translation is available.
            if (_configuration.TranslationProviders.Any())
            {
                virtualPathData =
                    this.GetTranslatedVirtualPath(t => ((HttpRoute)t).GetVirtualPath(request, values))
                    ?? virtualPathData;
            }

            // Lowercase, append trailing slash, etc.
            var virtualPath = this.GetFinalVirtualPath(virtualPathData.VirtualPath, _configuration);

            return new HttpVirtualPathData(virtualPathData.Route, virtualPath);
        }
    }
}
