using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Defines a POST route for an action in Mvc Controllers.
    /// </summary>
    public class POSTAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for a POST request.
        /// The route URL will be the name of the action.
        /// </summary>
        public POSTAttribute() : base(HttpVerbs.Post) {}

        /// <summary>
        /// Specify a route for a POST request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public POSTAttribute(string routeUrl) : base(routeUrl, HttpVerbs.Post) {}
    }
}