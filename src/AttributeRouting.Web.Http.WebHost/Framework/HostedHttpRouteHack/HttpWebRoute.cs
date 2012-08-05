// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace AttributeRouting.Web.Http.WebHost.Framework.HostedHttpRouteHack
{
    /// <summary>
    /// Mimics the System.Web.Routing.Route class to work better for Web API scenarios. The only
    /// difference between the base class and this class is that this one will match only when
    /// a special "httproute" key is specified when generating URLs. There is no special behavior
    /// for incoming URLs.
    /// </summary>
    public class HttpWebRoute : Route
    {
        /// <summary>
        /// Key used to signify that a route URL generation request should include HTTP routes (e.g. Web API).
        /// If this key is not specified then no HTTP routes will match.
        /// </summary>
        internal const string HttpRouteKey = "httproute";

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Matches the base class's parameter names.")]
        public HttpWebRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler, IHttpRoute httpRoute)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            if (httpRoute == null)
            {
                throw new ArgumentNullException("httpRoute");
            }

            HttpRoute = httpRoute;
        }

        /// <summary>
        /// Gets the <see cref="IHttpRoute"/> associated with this <see cref="HttpWebRoute"/>.
        /// </summary>
        public IHttpRoute HttpRoute { get; private set; }

        protected override bool ProcessConstraint(HttpContextBase httpContext, object constraint, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            IHttpRouteConstraint httpRouteConstraint = constraint as IHttpRouteConstraint;
            if (httpRouteConstraint != null)
            {
                HttpRequestMessage request = httpContext.GetHttpRequestMessage();
                if (request == null)
                {
                    var requestBase = httpContext.Request;
                    var httpMethod = GetHttpMethod(requestBase.HttpMethod);
                    var uri = requestBase.Url;
                    request = new HttpRequestMessage(httpMethod, uri);
                    httpContext.SetHttpRequestMessage(request);
                }

                return httpRouteConstraint.Match(request, HttpRoute, parameterName, values, ConvertRouteDirection(routeDirection));
            }

            return base.ProcessConstraint(httpContext, constraint, parameterName, values, routeDirection);
        }

        private HttpMethod GetHttpMethod(string method)
        {
            if (String.IsNullOrEmpty(method))
            {
                return null;
            }

            if (String.Equals("GET", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Get;
            }

            if (String.Equals("POST", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Post;
            }

            if (String.Equals("PUT", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Put;
            }

            if (String.Equals("DELETE", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Delete;
            }

            if (String.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Head;
            }

            if (String.Equals("OPTIONS", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Options;
            }

            if (String.Equals("TRACE", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Trace;
            }

            return new HttpMethod(method);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Only perform URL generation if the "httproute" key was specified. This allows these
            // routes to be ignored when a regular MVC app tries to generate URLs. Without this special
            // key an HTTP route used for Web API would normally take over almost all the routes in a
            // typical app.
            if (!values.ContainsKey(HttpRouteKey))
            {
                return null;
            }
            // Remove the value from the collection so that it doesn't affect the generated URL
            RouteValueDictionary newValues = GetRouteDictionaryWithoutHttpRouteKey(values);

            return base.GetVirtualPath(requestContext, newValues);
        }

        private static RouteValueDictionary GetRouteDictionaryWithoutHttpRouteKey(IDictionary<string, object> routeValues)
        {
            var newRouteValues = new RouteValueDictionary();
            foreach (var routeValue in routeValues)
            {
                if (!String.Equals(routeValue.Key, HttpRouteKey, StringComparison.OrdinalIgnoreCase))
                {
                    newRouteValues.Add(routeValue.Key, routeValue.Value);
                }
            }
            return newRouteValues;
        }

        private static HttpRouteDirection ConvertRouteDirection(RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.IncomingRequest)
            {
                return HttpRouteDirection.UriResolution;
            }

            if (routeDirection == RouteDirection.UrlGeneration)
            {
                return HttpRouteDirection.UriGeneration;
            }

            throw new InvalidEnumArgumentException("routeDirection", (int)routeDirection, typeof(RouteDirection));
        }
    }
}
