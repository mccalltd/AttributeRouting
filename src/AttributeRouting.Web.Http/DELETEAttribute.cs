using System.Web.Http;

namespace AttributeRouting.Web.Http
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of DELETE.
    /// </summary>
    public class DELETEAttribute : HttpDeleteAttribute
    {
        /// <summary>
        /// Specify a route for an action constrained to requests providing an httpMethod value of DELETE.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public DELETEAttribute(string routeUrl) : base(routeUrl, HttpVerbs.Delete) {}
    }
}