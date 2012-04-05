using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AttributeRouting.Http
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HttpRouteAttribute : Attribute, IRouteAttribute {
        public HttpRouteAttribute(string routeUrl, params string[] allowedMethods) {
            if (routeUrl == null) throw new ArgumentNullException("routeUrl");            

            RouteUrl = routeUrl;
            HttpMethods = allowedMethods;
            Order = int.MaxValue;
            Precedence = int.MaxValue;                        
        }

        /// <summary>
        /// The HttpMethods this route is constrained against.
        /// </summary>
        public string[] HttpMethods { get; private set; }

        /// <summary>
        /// The url for this action.
        /// </summary>
        public string RouteUrl { get; private set; }

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
    }
}
