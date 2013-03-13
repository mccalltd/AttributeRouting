using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Web.Http.Framework
{
    /// <summary>
    /// Route to use for Web API routes.
    /// </summary>
    public class HttpAttributeRoute : HttpRoute, IAttributeRoute
    {
        private const string RequestedPathKey = "__AttributeRouting:RequestedPath";
        private readonly AttributeRouteVisitor _visitor;

        /// <summary>
        /// Route used by the AttributeRouting framework in self-host projects.
        /// </summary>
        public HttpAttributeRoute(string url,
                                  HttpRouteValueDictionary defaults,
                                  HttpRouteValueDictionary constraints,
                                  HttpRouteValueDictionary dataTokens,
                                  HttpConfiguration configuration)
            : base(url, defaults, constraints, dataTokens, configuration.MessageHandler)
        {
            _visitor = new AttributeRouteVisitor(this);
            QueryStringConstraints = new RouteValueDictionary();
            QueryStringDefaults = new RouteValueDictionary();
        }

        IDictionary<string, object> IAttributeRoute.Constraints
        {
            get { return Constraints; }
            set { throw new NotImplementedException("HttpRoute.Constraints has no setter."); }
        }

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

        public IDictionary<string, object> QueryStringConstraints { get; set; }
        
        public IDictionary<string, object> QueryStringDefaults { get; set; }

        public string RouteName { get; set; }

        public string Url
        {
            get { return RouteTemplate; }
            set { throw new NotImplementedException("IHttpRoute.RouteTemplate has no setter."); }
        }

        public override IHttpRouteData GetRouteData(string virtualPathRoot, HttpRequestMessage request)
        {
            // Optimize matching by comparing the static left part of the route url with the requested path.
            var requestedPath = GetCachedValue(request, RequestedPathKey, () => request.RequestUri.AbsolutePath.Substring(1).TrimEnd('/'));
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

            return routeData;
        }

        public override IHttpVirtualPathData GetVirtualPath(HttpRequestMessage request, IDictionary<string, object> values)
        {
            // Add querystring default values if applicable.
            _visitor.AddQueryStringDefaultsToRouteValues(values);

            // Let the underlying route do its thing.
            var virtualPathData = base.GetVirtualPath(request, values);
            if (virtualPathData == null)
            {
                return null;
            }

            return new HttpVirtualPathData(virtualPathData.Route, virtualPathData.VirtualPath);
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
