namespace AttributeRouting
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of PUT.
    /// </summary>
    public class PUTAttribute : RouteAttributeBase
    {
        /// <summary>
        /// Specify a route for an action constrained to requests providing an httpMethod value of PUT.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public PUTAttribute(string routeUrl) : base(routeUrl, "PUT") {}
    }
}