using System;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// The route information for an action.
    /// </summary>
    public class RouteAttribute : RouteAttributeBase
    {
        public RouteAttribute(string routeUrl)
            : base(routeUrl)
        {
        }

        public RouteAttribute(string routeUrl, HttpVerbs allowedMethods)
            : base(routeUrl)
        {
            HttpMethods = allowedMethods.ToString().ToUpper().SplitAndTrim(new[] {","});
        }
    }
}