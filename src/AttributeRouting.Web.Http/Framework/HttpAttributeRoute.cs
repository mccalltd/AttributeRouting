using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.Framework
{
    /// <summary>
    /// Route to use for Web API routes.
    /// </summary>
    public class HttpAttributeRoute : HttpRoute, IAttributeRoute
    {
        private const string RequestedPathKey = "__AttributeRouting:RequestedPath";
        private const string RequestedSubdomainKey = "__AttributeRouting:RequestedSubdomain";
        private const string CurrentUICultureNameKey = "__AttributeRouting:CurrentUICulture";

        private readonly HttpConfigurationBase _configuration;
        private readonly AttributeRouteVisitor _visitor;

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
            _visitor = new AttributeRouteVisitor(this, configuration);
            QueryStringConstraints = new RouteValueDictionary();
        }

        public bool? AppendTrailingSlash { get; set; }

        IDictionary<string, object> IAttributeRoute.Constraints
        {
            get { return Constraints; }
            set { throw new NotImplementedException("HttpRoute.Constraints has no setter."); }
        }

        public string CultureName { get; set; }

        IDictionary<string, object> IAttributeRoute.DataTokens
        {
            get { return DataTokens; }
            set { throw new NotImplementedException("HttpRoute.DataTokens has no setter."); }
        }

        IDictionary<string, object> IAttributeRoute.Defaults
        {
            get { return Defaults; }
            set { throw new NotImplementedException("HttpRoute.Defaults has no setter."); }
        }

        public List<string> MappedSubdomains { get; set; }

        public bool? PreserveCaseForUrlParameters { get; set; }
        
        public IDictionary<string, object> QueryStringConstraints { get; set; }

        public string RouteName { get; set; }

        public IAttributeRoute SourceLanguageRoute { get; set; }

        public string Subdomain { get; set; }

        public IEnumerable<IAttributeRoute> Translations { get; set; }

        public string Url
        {
            get { return RouteTemplate; }
            set { throw new NotImplementedException("IHttpRoute.RouteTemplate has no setter."); }
        }

        public bool? UseLowercaseRoute { get; set; }

        public override IHttpRouteData GetRouteData(string virtualPathRoot, HttpRequestMessage request)
        {
            // Optimize matching by comparing the static left part of the route url with the requested path.
            var requestedPath = GetCachedValue(request, RequestedPathKey, () => request.RequestUri.AbsolutePath.Substring(1));
            if (!_visitor.IsStaticLeftPartOfUrlMatched(requestedPath))
            {
                return null;
            }

            // Let the underlying route match, and if it does, then add a few more constraints.
            var routeData = base.GetRouteData(virtualPathRoot, request);
            if (routeData == null)
            {
                return null;
            }

            // Constrain by querystring param if there are any.
            var routeValues = new HttpRouteValueDictionary(routeData.Values);
            if (!_visitor.ProcessQueryStringConstraints((constraint, parameterName) => ProcessConstraint(request, constraint, parameterName, routeValues, HttpRouteDirection.UriResolution)))
            {
                return null;
            }

            // Constrain by subdomain if configured
            var requestedSubdomain = GetCachedValue(request, RequestedSubdomainKey, () => _configuration.SubdomainParser(request.SafeGet(r => r.Headers.Host)));
            if (!_visitor.IsSubdomainMatched(requestedSubdomain))
            {
                return null;
            }

            // Constrain by culture name if configured
            var currentUICultureName = GetCachedValue(request, CurrentUICultureNameKey, () => _configuration.CurrentUICultureResolver(request, routeData));
            if (!_visitor.IsCultureNameMatched(currentUICultureName))
            {
                return null;
            }

            return routeData;
        }

        public override IHttpVirtualPathData GetVirtualPath(HttpRequestMessage request, IDictionary<string, object> values)
        {
            // Let the underlying route do its thing.
            var virtualPathData = base.GetVirtualPath(request, values);
            if (virtualPathData == null)
            {
                return null;
            }

            // Translate this path if a translation is available.
            var translatedVirtualPath = _visitor.GetTranslatedVirtualPath(t => ((HttpRoute)t).GetVirtualPath(request, values));
            if (translatedVirtualPath != null)
            {
                virtualPathData = translatedVirtualPath;
            }

            // Lowercase, append trailing slash, etc.
            var virtualPath = _visitor.GetFinalVirtualPath(virtualPathData.VirtualPath);

            return new HttpVirtualPathData(virtualPathData.Route, virtualPath);
        }

        private static T GetCachedValue<T>(HttpRequestMessage context, string key, Func<T> initializeValue)
        {
            // Fetch the item from the http context if it's been stored for the request.
            if (context.Properties.ContainsKey(key))
            {
                return (T)context.Properties[key];
            }

            // Cache the value and return it.
            var value = initializeValue();
            context.Properties.Add(key, value);
            return value;
        }
    }
}
