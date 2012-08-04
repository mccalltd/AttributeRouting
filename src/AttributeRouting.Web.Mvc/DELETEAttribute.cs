using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of DELETE.
    /// </summary>
    public class DELETEAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for DELETE request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public DELETEAttribute(string routeUrl) : base(routeUrl, HttpVerbs.Delete) {}
    }
}