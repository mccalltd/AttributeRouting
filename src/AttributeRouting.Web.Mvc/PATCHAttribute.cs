namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Defines a PATCH route for an action in Mvc Controllers.
    /// </summary>
    public class PATCHAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for PATCH request.
        /// The route URL will be the name of the action.
        /// </summary>
        public PATCHAttribute() : base("PATCH") { }

        /// <summary>
        /// Specify a route for PATCH request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public PATCHAttribute(string routeUrl) : base(routeUrl, "PATCH") { }
    }
}