namespace AttributeRouting
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of POST.
    /// </summary>
    public class POSTAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for an action constrained to requests providing an httpMethod value of POST.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        public POSTAttribute(string url) : base(url, "POST") {}
    }
}