namespace AttributeRouting
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of PUT.
    /// </summary>
    public class PUTAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for an action constrained to requests providing an httpMethod value of PUT.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        public PUTAttribute(string url) : base(url, "PUT") {}
    }
}