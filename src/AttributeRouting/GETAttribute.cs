namespace AttributeRouting
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of GET.
    /// </summary>
    public class GETAttribute : RouteAttributeBase
    {
        /// <summary>
        /// Specify a route for an action constrained to requests providing an httpMethod value of GET.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public GETAttribute(string routeUrl) : base(routeUrl, "GET", "HEAD") {}
    }
}