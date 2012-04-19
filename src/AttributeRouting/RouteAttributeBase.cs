using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AttributeRouting
{
    /// <summary>
    /// The route information for an action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class RouteAttributeBase : Attribute
    {
        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        protected RouteAttributeBase(string routeUrl)
        {
            if (routeUrl == null) throw new ArgumentNullException("routeUrl");

            RouteUrl = routeUrl;
            Order = int.MaxValue;
            Precedence = int.MaxValue;
            HttpMethods = new string[0];
        }

        /// <summary>
        /// Specify the route information for an action.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        /// <param name="allowedMethods">The httpMethods against which to constrain the route</param>
        protected RouteAttributeBase(string routeUrl, params string[] allowedMethods)
            : this(routeUrl)
        {
            HttpMethods = allowedMethods;

            if (HttpMethods.Any(m => !Regex.IsMatch(m, "HEAD|GET|POST|PUT|DELETE|PATCH|OPTIONS|TRACE")))
                throw new InvalidOperationException(
                    "The allowedMethods are restricted to either HEAD, GET, POST, PUT, DELETE, PATCH, OPTIONS, or TRACE.");
        }

        /// <summary>
        /// The url for this action.
        /// </summary>
        public string RouteUrl { get; private set; }

        /// <summary>
        /// The HttpMethods this route is constrained against.
        /// </summary>
        public string[] HttpMethods { get; protected set; }

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

        /// <summary>
        /// If set, will override <see cref="AttributeRoutingConfigurationBase.UseLowercaseRoutes"/>
        /// set via global configuration for this route.
        /// </summary>
        public bool UseLowercaseRoute
        {
            get { return UseLowercaseRouteFlag.GetValueOrDefault(); }
            set { UseLowercaseRouteFlag = value; }
        }

        /// <summary>
        /// Internal flag marking the tri-state value.
        /// </summary>
        internal bool? UseLowercaseRouteFlag { get; set; }

        /// <summary>
        /// If set, will override <see cref="AttributeRoutingConfigurationBase.PreserveCaseForUrlParameters"/>
        /// set via global configuration for this route.
        /// </summary>
        public bool PreserveCaseForUrlParameters 
        {
            get { return PreserveCaseForUrlParametersFlag.GetValueOrDefault(); }
            set { PreserveCaseForUrlParametersFlag = value; }
        }

        /// <summary>
        /// Internal flag marking the tri-state value.
        /// </summary>
        internal bool? PreserveCaseForUrlParametersFlag { get; set; }

        /// <summary>
        /// If true, will override <see cref="AttributeRoutingConfigurationBase.AppendTrailingSlash"/>
        /// set via global configuration for this route.
        /// </summary>
        public bool AppendTrailingSlash
        {
            get { return AppendTrailingSlashFlag.GetValueOrDefault(); }
            set { AppendTrailingSlashFlag = value; }
        }

        /// <summary>
        /// Internal flag marking the tri-state value.
        /// </summary>
        internal bool? AppendTrailingSlashFlag { get; set; }
    }
}