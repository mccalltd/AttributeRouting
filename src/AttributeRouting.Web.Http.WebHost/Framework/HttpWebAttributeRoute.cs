using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Web;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Framework;
using AttributeRouting.Web.Http.WebHost.Routing;

namespace AttributeRouting.Web.Http.WebHost.Framework
{
    /// <summary>
    /// Route to use for Web API web-hosted routes.
    /// </summary>
    /// <remarks>
    /// This class is intended to mimic the functionality of System.Web.Http.WebHost.Routing.HttpWebRoute,
    /// which is internal to the framework.
    /// </remarks>
    public class HttpWebAttributeRoute : AttributeRoute
    {
        /// <summary>
        /// Key used to signify that a route URL generation request should include HTTP routes (e.g. Web API).
        /// If this key is not specified then no HTTP routes will match.
        /// </summary>
        internal const string HttpRouteKey = "httproute";
        internal static readonly string HttpContextBaseKey = "MS_HttpContext";

        /// <summary>
        /// Route used by the AttributeRouting framework in web projects.
        /// </summary>
        public HttpWebAttributeRoute(string url,
                                     RouteValueDictionary defaults,
                                     RouteValueDictionary constraints,
                                     RouteValueDictionary dataTokens,
                                     HttpWebAttributeRoutingConfiguration configuration)
            : base(url, defaults, constraints, dataTokens, configuration)
        {
            HttpRoute = new HttpRoute(url,
                                      new HttpRouteValueDictionary(defaults),
                                      new HttpRouteValueDictionary(constraints),
                                      new HttpRouteValueDictionary(dataTokens));
        }

        /// <summary>
        /// Gets the <see cref="IHttpRoute"/> associated with this <see cref="HttpWebAttributeRoute"/>.
        /// </summary>
        public IHttpRoute HttpRoute { get; private set; }

        protected override bool ProcessConstraint(HttpContextBase httpContext, object constraint, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var httpRouteConstraint = constraint as IHttpRouteConstraint;
            if (httpRouteConstraint != null)
            {
                HttpRequestMessage request = httpContext.GetHttpRequestMessage();
                if (request == null)
                {
                    // Seeing how it goes without re-implementing .ConvertRequest
                    // request = HttpControllerHandler.ConvertRequest(httpContext);
                    // httpContext.SetHttpRequestMessage(request);

                    request = ConvertRequest(httpContext);
                    httpContext.SetHttpRequestMessage(request);
                }

                return httpRouteConstraint.Match(request, HttpRoute, parameterName, values, ConvertRouteDirection(routeDirection));
            }

            return base.ProcessConstraint(httpContext, constraint, parameterName, values, routeDirection);
        }

        private HttpRequestMessage ConvertRequest(HttpContextBase httpContextBase)
        {

            var requestBase = httpContextBase.Request;
            var httpMethod = HttpMethodHelper.GetHttpMethod(requestBase.HttpMethod);
            var requestUri = requestBase.Url;

            var request = new HttpRequestMessage(httpMethod, requestUri);

            // Add context to enable route lookup later on
            request.Properties.Add(HttpContextBaseKey, httpContextBase);

            // Add information about whether the request is local or not
            request.Properties.Add(HttpPropertyKeys.IsLocalKey, new Lazy<bool>(() => requestBase.IsLocal));

            // Add information about whether custom errors are enabled for this request or not
            request.Properties.Add(HttpPropertyKeys.IncludeErrorDetailKey,
                                   new Lazy<bool>(() => !httpContextBase.IsCustomErrorEnabled));

            return request;
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
            var newValues = GetRouteDictionaryWithoutHttpRouteKey(values);

            return base.GetVirtualPath(requestContext, newValues);
        }

        private static RouteValueDictionary GetRouteDictionaryWithoutHttpRouteKey(IDictionary<string, object> routeValues)
        {
            var newRouteValues = new RouteValueDictionary();
            
            foreach (var routeValue in routeValues)
            {
                if (!routeValue.Key.ValueEquals(HttpRouteKey))
                    newRouteValues.Add(routeValue.Key, routeValue.Value);
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
