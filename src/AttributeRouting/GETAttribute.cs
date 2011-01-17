namespace AttributeRouting
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of GET.
    /// </summary>
    public class GETAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for an action constrained to requests providing an httpMethod value of POST.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        public GETAttribute(string url) : base(url, "GET") {}
    }
}