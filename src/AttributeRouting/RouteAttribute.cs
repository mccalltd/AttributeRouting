using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AttributeRouting.Extensions;

namespace AttributeRouting
{
    /// <summary>
    /// The route information for an action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RouteAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        public RouteAttribute(string url, params string[] allowedMethods)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (Regex.IsMatch(url, @"^\/|\/$") || !url.IsValidUrl(allowTokens: true))
                throw new ArgumentException(
                    ("The url \"{0}\" is not valid. It cannot start or end with forward slashes " +
                     "or contain any other character not allowed in URLs.").FormatWith(url));

            if (allowedMethods == null) throw new ArgumentNullException("allowedMethods");
            if (allowedMethods.Any(m => !Regex.IsMatch(m, "HEAD|GET|POST|PUT|DELETE")))
                throw new ArgumentException("The allowedMethods are restricted to either HEAD, GET, POST, PUT, or DELETE.", "allowedMethods");

            Url = url;
            HttpMethods = allowedMethods;
            Order = int.MaxValue;
            Precedence = int.MaxValue;
        }

        /// <summary>
        /// The url for this action.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// The HttpMethods this route is constrained against.
        /// </summary>
        public string[] HttpMethods { get; private set; }

        /// <summary>
        /// The order of this route among all the routes defined against this action.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The order of this route among all the routes defined against this controller.
        /// </summary>
        public int Precedence { get; set; }

        /// <summary>
        /// The name this route will be registered with in the RouteTable.
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

            return HttpMethods.Contains(httpMethod);
        }
    }
}