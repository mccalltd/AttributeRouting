namespace AttributeRouting
{
    public class POSTAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for this action constrained to requests providing an httpMethod value of POST.
        /// </summary>
        /// <param name="url">The url that is associated with this action</param>
        public POSTAttribute(string url) : base(url, "POST") {}
    }
}