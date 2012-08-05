using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Defines a GET route for an action in Mvc Controllers.
    /// </summary>
    public class GETAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for a GET request.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public GETAttribute(string routeUrl) : base(routeUrl, HttpVerbs.Get | HttpVerbs.Head) {}
    }
}