using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace AttributeRouting.Mvc
{
    /// <summary>
    /// The route information for an action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RouteAttribute : ActionMethodSelectorAttribute, IRouteAttribute
    {
        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        public RouteAttribute(string routeUrl, params string[] allowedMethods)
        {
            if (routeUrl == null) throw new ArgumentNullException("routeUrl");

            if (allowedMethods.Any(m => !Regex.IsMatch(m, "HEAD|GET|POST|PUT|DELETE")))
                throw new ArgumentException("The allowedMethods are restricted to either HEAD, GET, POST, PUT, or DELETE.", "allowedMethods");

            RouteUrl = routeUrl;
            HttpMethods = allowedMethods;
            Order = int.MaxValue;
            Precedence = int.MaxValue;
        }

        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        public RouteAttribute(string routeUrl, HttpVerbs allowedMethods)
            : this(routeUrl, (string[]) allowedMethods.ToString().ToUpper().SplitAndTrim(new[] { "," }))
        {
        }

        /// <summary>
        /// The url for this action.
        /// </summary>
        public string RouteUrl { get; private set; }

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

        /// <summary>
        /// Key used by translation provider to lookup the translation for the <see cref="RouteUrl"/>.
        /// </summary>
        public string TranslationKey { get; set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            if (!HttpMethods.Any())
                return true;

            var httpMethod = (string)(controllerContext.RouteData.Values["httpMethod"] ??
                                      controllerContext.HttpContext.Request.GetHttpMethod());

            return HttpMethods.Contains(httpMethod);
        }
    }
}