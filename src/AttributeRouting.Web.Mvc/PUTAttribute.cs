using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Defines a PUT route for an action in Mvc Controllers.
    /// </summary>
    public class PUTAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for a PUT request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public PUTAttribute(string routeUrl) : base(routeUrl, HttpVerbs.Put) {}
    }
}