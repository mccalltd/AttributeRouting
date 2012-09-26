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
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public RouteAttribute(string routeUrl)
        {
            if (routeUrl == null) throw new ArgumentNullException("routeUrl");

            RouteUrl = routeUrl;
            HttpMethods = new string[0];
            ActionPrecedence = int.MaxValue;
            ControllerPrecedence = int.MaxValue;
            SitePrecedence = int.MaxValue;
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

        public string RouteUrl { get; private set; }

        public string[] HttpMethods { get; protected set; }

        [Obsolete("Prefer ActionPrecedence for clarity of intent.")]
        public int Order
        {
            get { return ActionPrecedence; }
            set { ActionPrecedence = value; }
        }
        
        public int ActionPrecedence { get; set; }

        [Obsolete("Prefer ControllerPrecedence for clarity of intent.")]
        public int Precedence
        {
            get { return ControllerPrecedence; }
            set { ControllerPrecedence = value; }
        }

        public int ControllerPrecedence { get; set; }

        public int SitePrecedence { get; set; }

        public string RouteName { get; set; }

        public bool IsAbsoluteUrl { get; set; }

        public string TranslationKey { get; set; }

        public bool UseLowercaseRoute
        {
            get { return UseLowercaseRouteFlag.GetValueOrDefault(); }
            set { UseLowercaseRouteFlag = value; }
        }

        public bool? UseLowercaseRouteFlag { get; private set; }

        public bool PreserveCaseForUrlParameters 
        {
            get { return PreserveCaseForUrlParametersFlag.GetValueOrDefault(); }
            set { PreserveCaseForUrlParametersFlag = value; }
        }
        
        public bool? PreserveCaseForUrlParametersFlag { get; private set; }

        public bool AppendTrailingSlash
        {
            get { return AppendTrailingSlashFlag.GetValueOrDefault(); }
            set { AppendTrailingSlashFlag = value; }
        }

        public bool? AppendTrailingSlashFlag { get; private set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            if (!HttpMethods.Any())
                return true;

            var method = controllerContext.HttpContext.Request.GetHttpMethodOverride();
            return HttpMethods.Any(m => m.ValueEquals(method));
        }
    }
}