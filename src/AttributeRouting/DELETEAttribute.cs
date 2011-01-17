namespace AttributeRouting
{
    public class DELETEAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for this action constrained to requests providing an httpMethod value of DELETE.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        public DELETEAttribute(string url) : base(url, "DELETE") {}
    }
}