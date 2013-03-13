using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// The route information for an action in Mvc Controllers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RouteAttribute : ActionMethodSelectorAttribute, IRouteAttribute
    {
        /// <summary>
        /// Specify the route information for an action. 
        /// The route URL will be the name of the action.
        /// </summary>
        public RouteAttribute()
        {
            HttpMethods = new string[0];
            ActionPrecedence = int.MaxValue;
            ControllerPrecedence = int.MaxValue;
            SitePrecedence = int.MaxValue;
        }

        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public RouteAttribute(string routeUrl)
            : this()
        {
            RouteUrl = routeUrl;
        }

        /// <summary>
        /// Specify the route information for an action.
        /// The route URL will be the name of the action.
        /// </summary>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        public RouteAttribute(params HttpVerbs[] allowedMethods)
            : this()
        {
            HttpMethods = allowedMethods.Select(m => m.ToString().ToUpperInvariant()).ToArray();
        }

        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        public RouteAttribute(string routeUrl, params HttpVerbs[] allowedMethods)
            : this(routeUrl)
        {
            HttpMethods = allowedMethods.Select(m => m.ToString().ToUpperInvariant()).ToArray();
        }

        public int ControllerPrecedence { get; set; }
        
        public string[] HttpMethods { get; protected set; }

        public int ActionPrecedence { get; set; }

        public bool IgnoreAreaUrl { get; set; }

        public bool IsAbsoluteUrl
        {
            get { return IgnoreAreaUrl && IgnoreRoutePrefix; }
            set { IgnoreAreaUrl = IgnoreRoutePrefix = value; }
        }

        public bool IgnoreRoutePrefix { get; set; }

        public string RouteName { get; set; }

        public string RouteUrl { get; private set; }

        public int SitePrecedence { get; set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            if (controllerContext == null) throw new ArgumentNullException("controllerContext");

            // If not constrained by a method, then accept always!
            if (!HttpMethods.Any())
                return true;

            var httpMethod = controllerContext.HttpContext.Request.GetHttpMethodOverride();
            
            return HttpMethods.Any(m => m.ValueEquals(httpMethod));
        }
    }
}