namespace AttributeRouting
{
    public class GETAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for this action constrained to requests providing an httpMethod value of GET.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        public GETAttribute(string url) : base(url, "GET") {}
    }
}