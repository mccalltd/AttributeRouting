namespace AttributeRouting
{
    public class PUTAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for this action constrained to requests providing an httpMethod value of PUT.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        public PUTAttribute(string url) : base(url, "PUT") {}
    }
}