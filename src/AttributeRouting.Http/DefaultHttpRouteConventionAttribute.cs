using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Http
{
    /// <summary>
    /// Sets up the default Web API route convention
    /// See: http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api
    /// </summary>
    public class DefaultHttpRouteConventionAttribute : HttpConventionAttribute
    {
        // Setup conventions
        private static readonly List<HttpRouteConventionInfo> Conventions = new List<HttpRouteConventionInfo>
        {
            // api/products (GET)
            new HttpRouteConventionInfo(HttpMethod.Get, ""),
            // api/products/{id} (GET)
            new HttpRouteConventionInfo(HttpMethod.Get, "{id}"),
            // api/products (POST)
            new HttpRouteConventionInfo(HttpMethod.Post, ""),
            // api/products/{id} (DELETE)
            new HttpRouteConventionInfo(HttpMethod.Delete, "{id}"),
            // api/products/{id} (PUT)
            new HttpRouteConventionInfo(HttpMethod.Put, "{id}")
        };

        public override IEnumerable<IRouteAttribute> GetRouteAttributes(MethodInfo actionMethod)
        {
            foreach (var c in Conventions)
            {
                if (actionMethod.Name.StartsWith(c.HttpMethod.Method, StringComparison.OrdinalIgnoreCase))
                {
                    var requiresId = !string.IsNullOrEmpty(c.Url);

                    if (!c.AlreadyUsed)
                    {
                        // Check first parameter, if it requires ID
                        if (!requiresId || (actionMethod.GetParameters().Length > 0 && actionMethod.GetParameters()[0].Name.Equals("id", StringComparison.OrdinalIgnoreCase)))
                        {
                            yield return BuildRouteAttribute(c);

                            c.AlreadyUsed = true;
                        }
                    }
                }
            }
        }

        public override string GetDefaultRoutePrefix(MethodInfo actionMethod)
        {
            return actionMethod.DeclaringType.GetControllerName();
        }

        public override IEnumerable<RouteDefaultAttribute> GetRouteDefaultAttributes(MethodInfo actionMethod)
        {
            yield break;
        }

        private IRouteAttribute BuildRouteAttribute(HttpRouteConventionInfo convention)
        {
            switch (convention.HttpMethod.Method)
            {
                case "GET":
                    return new GETAttribute(convention.Url);
                case "POST":
                    return new POSTAttribute(convention.Url);
                case "PUT":
                    return new PUTAttribute(convention.Url);
                case "DELETE":
                    return new DELETEAttribute(convention.Url);
                default:
                    throw new AttributeRoutingException(StringExtensions.FormatWith("Unknown HTTP method \"{0}\".", convention.HttpMethod));
            }
        }

        private class HttpRouteConventionInfo
        {
            public HttpRouteConventionInfo(HttpMethod httpMethod, string url)
            {
                HttpMethod = httpMethod;
                Url = url;
                AlreadyUsed = false;
            }

            public bool AlreadyUsed { get; set; }
            public HttpMethod HttpMethod { get; private set; }
            public string Url { get; private set; }
        }
    }
}
