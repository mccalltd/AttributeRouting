using System.Net.Http;

namespace AttributeRouting.Web.Http
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of PATCH.
    /// </summary>
    public class PATCHAttribute : HttpRouteAttribute
    {
        /// <summary>
        /// Specify a route for a PATCH request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public PATCHAttribute(string routeUrl) : base(routeUrl, new HttpMethod("PATCH")) { }
    }
}