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
        /// <param name="routeUrl">The url that is associated with this action</param>
        public HttpRouteAttribute(string routeUrl)
        {
            if (routeUrl == null) throw new ArgumentNullException("routeUrl");

            RouteUrl = routeUrl;
            SitePrecedence = int.MaxValue;
            HttpMethods = new string[0];
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

        public string RouteUrl { get; private set; }

        public string[] HttpMethods { get; protected set; }

        public int Order { get; set; }

        public int Precedence { get; set; }

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
    }
}