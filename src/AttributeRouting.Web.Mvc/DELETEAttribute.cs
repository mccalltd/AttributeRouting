using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Defines a DELETE route for an action in Mvc Controllers.
    /// </summary>
    public class DELETEAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for DELETE request.
        /// The route URL will be the name of the action.
        /// </summary>
        public DELETEAttribute() : base(HttpVerbs.Delete) {}

        /// <summary>
        /// Specify a route for DELETE request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public DELETEAttribute(string routeUrl) : base(routeUrl, HttpVerbs.Delete) {}
    }
}