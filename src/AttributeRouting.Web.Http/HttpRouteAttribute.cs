using System;
using System.Linq;
using System.Net.Http;

namespace AttributeRouting.Web.Http
{
    /// <summary>
    /// The route information for an action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HttpRouteAttribute : Attribute, IRouteAttribute
    {
        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        public HttpRouteAttribute()
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
        public HttpRouteAttribute(string routeUrl)
            : this()
        {
            RouteUrl = routeUrl;
        }

        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        public HttpRouteAttribute(params HttpMethod[] allowedMethods)
            : this()
        {
            HttpMethods = allowedMethods.Select(m => m.Method.ToUpper()).ToArray();
        }

        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        public HttpRouteAttribute(string routeUrl, params HttpMethod[] allowedMethods)
            : this(routeUrl)
        {
            HttpMethods = allowedMethods.Select(m => m.Method.ToUpper()).ToArray();
        }

        public int ActionPrecedence { get; set; }

        public int ControllerPrecedence { get; set; }

        public string[] HttpMethods { get; protected set; }

        public bool IgnoreAreaUrl { get; set; }

        public bool IgnoreRoutePrefix { get; set; }

        public bool IsAbsoluteUrl
        {
            get { return IgnoreAreaUrl && IgnoreRoutePrefix; }
            set { IgnoreAreaUrl = IgnoreRoutePrefix = value; }
        }

        [Obsolete("Prefer ActionPrecedence for clarity of intent.")]
        public int Order
        {
            get { return ActionPrecedence; }
            set { ActionPrecedence = value; }
        }

        [Obsolete("Prefer ControllerPrecedence for clarity of intent.")]
        public int Precedence
        {
            get { return ControllerPrecedence; }
            set { ControllerPrecedence = value; }
        }

        public string RouteName { get; set; }

        public string RouteUrl { get; private set; }

        public int SitePrecedence { get; set; }
    }
}