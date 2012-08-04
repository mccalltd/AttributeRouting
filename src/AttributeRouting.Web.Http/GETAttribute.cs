using System.Net.Http;

namespace AttributeRouting.Web.Http
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of GET.
    /// </summary>
    public class GETAttribute : HttpRouteAttribute
    {
        /// <summary>
        /// Specify a route for a GET request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public GETAttribute(string routeUrl) : base(routeUrl, HttpMethod.Get, HttpMethod.Head, HttpMethod.Options) {}
    }
}