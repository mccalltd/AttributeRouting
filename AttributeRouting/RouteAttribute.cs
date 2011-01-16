using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RouteAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// Specify the route information for this action.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        /// <param name="httpMethod">The httpMethod against which to constrain the route</param>
        public RouteAttribute(string url, string httpMethod)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (Regex.IsMatch(url, @"^\/|\/$") || !url.IsValidUrl(true))
                throw new ArgumentException(
                    ("The url \"{0}\" is not valid. It cannot start or end with forward slashes " +
                     "or contain any other character not allowed in URLs.").FormatWith(url), "url");

            if (httpMethod == null) throw new ArgumentNullException("httpMethod");
            if (!Regex.IsMatch(httpMethod, "GET|POST|PUT|DELETE"))
                throw new ArgumentException("The httpMethod must be either GET, POST, PUT, or DELETE.", "httpMethod");

            Url = url;
            HttpMethod = httpMethod;
            Order = int.MaxValue;
        }

        /// <summary>
        /// The url for this action.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// The HttpMethod this route is constrained against.
        /// </summary>
        public string HttpMethod { get; private set; }

        /// <summary>
        /// The order of this route among all the routes defined against this action.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The name of this route will be registered with in the RouteTable.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// If true, the generated route url will be applied from the root, skipping any relevant area name or route prefix.
        /// </summary>
        public bool IsAbsoluteUrl { get; set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var httpMethod = (string)(controllerContext.RouteData.Values["httpMethod"] ??
                                      controllerContext.HttpContext.Request.GetHttpMethod());

            return httpMethod.ValueEquals(HttpMethod);
        }
    }
}