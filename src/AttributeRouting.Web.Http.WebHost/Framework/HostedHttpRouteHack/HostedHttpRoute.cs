// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace AttributeRouting.Web.Http.WebHost.Framework.HostedHttpRouteHack
{
    public class HostedHttpRoute : IHttpRoute
    {
        public HostedHttpRoute(string uriTemplate, IDictionary<string, object> defaults, IDictionary<string, object> constraints, IDictionary<string, object> dataTokens, HttpMessageHandler handler)
        {
            RouteValueDictionary routeDefaults = defaults != null ? new RouteValueDictionary(defaults) : null;
            RouteValueDictionary routeConstraints = constraints != null ? new RouteValueDictionary(constraints) : null;
            RouteValueDictionary routeDataTokens = dataTokens != null ? new RouteValueDictionary(dataTokens) : null;
            OriginalRoute = new HttpWebRoute(uriTemplate, routeDefaults, routeConstraints, routeDataTokens, HttpControllerRouteHandler.Instance, this);
            Handler = handler;
        }

        public string RouteTemplate
        {
            get { return OriginalRoute.Url; }
        }

        public IDictionary<string, object> Defaults
        {
            get { return OriginalRoute.Defaults; }
        }

        public IDictionary<string, object> Constraints
        {
            get { return OriginalRoute.Constraints; }
        }

        public IDictionary<string, object> DataTokens
        {
            get { return OriginalRoute.DataTokens; }
        }

        public HttpMessageHandler Handler { get; private set; }

        internal Route OriginalRoute { get; private set; }

        public IHttpRouteData GetRouteData(string rootVirtualPath, HttpRequestMessage request)
        {
            if (rootVirtualPath == null)
            {
                throw new ArgumentNullException("rootVirtualPath");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            HttpContextBase httpContextBase;
            if (!request.Properties.TryGetValue(Constants.HttpContextBaseKey, out httpContextBase))
            {
                httpContextBase = new HttpRequestMessageContextWrapper(rootVirtualPath, request);
            }

            RouteData routeData = OriginalRoute.GetRouteData(httpContextBase);
            if (routeData != null)
            {
                return new HostedHttpRouteData(routeData);
            }

            return null;
        }

        public IHttpVirtualPathData GetVirtualPath(HttpRequestMessage request, IDictionary<string, object> values)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            HttpContextBase httpContextBase;
            if (request.Properties.TryGetValue(Constants.HttpContextBaseKey, out httpContextBase))
            {
                HostedHttpRouteData routeData = request.GetRouteData() as HostedHttpRouteData;
                if (routeData != null)
                {
                    RequestContext requestContext = new RequestContext(httpContextBase, routeData.OriginalRouteData);
                    VirtualPathData virtualPathData = OriginalRoute.GetVirtualPath(requestContext, new RouteValueDictionary(values));
                    if (virtualPathData != null)
                    {
                        return new HostedHttpVirtualPathData(virtualPathData, routeData.Route);
                    }
                }
            }

            return null;
        }
    }
}
