using System.Web.Mvc;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Defines a route for an action constrained to requests providing an httpMethod value of GET.
    /// </summary>
    public class GETAttribute : RouteAttribute
    {
        /// <summary>
        /// Specify a route for an action constrained to requests providing an httpMethod value of GET.
        /// </summary>
        /// <param name="routeUrl">The url that is associated with this action</param>
        public GETAttribute(string routeUrl) : base(routeUrl, HttpVerbs.Get | HttpVerbs.Head) {}
    }
}