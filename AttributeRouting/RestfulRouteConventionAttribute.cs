using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AttributeRouting
{
    public class RestfulRouteConventionAttribute : RouteConventionAttribute
    {
        // Setup conventions
        private static readonly List<RestfulRouteConventionInfo> Conventions = new List<RestfulRouteConventionInfo>
        {
            new RestfulRouteConventionInfo("Index", "GET", ""),
            new RestfulRouteConventionInfo("New", "GET", "New"),
            new RestfulRouteConventionInfo("Create", "POST", ""),
            new RestfulRouteConventionInfo("Show", "GET", "{id}"),
            new RestfulRouteConventionInfo("Edit", "GET", "{id}/Edit"),
            new RestfulRouteConventionInfo("Update", "PUT", "{id}"),
            new RestfulRouteConventionInfo("Delete", "GET", "{id}/Delete"),
            new RestfulRouteConventionInfo("Destroy", "DELETE", "{id}")
        };

        public override IEnumerable<RouteSpecification> BuildRoutes(IEnumerable<RouteSpecification> routeSpecs)
        {
            foreach (var routeSpec in routeSpecs)
            {
                // Do not override any routes already defined via a RouteAttribute
                if (routeSpec.Url != null)
                    yield return routeSpec;

                // Handle conventional actions
                var convention = Conventions.SingleOrDefault(c => c.ActionName == routeSpec.ActionName);
                if (convention != null)
                {
                    routeSpec.HttpMethod = convention.HttpMethod;
                    routeSpec.Url = convention.Url;

                    yield return routeSpec;
                }
                
                // If the route spec does not pertain to an explictly defined route or a conventional route, 
                // then do not yield.
            }
        }

        private class RestfulRouteConventionInfo
        {
            public RestfulRouteConventionInfo(string actionName, string httpMethod, string url)
            {
                ActionName = actionName;
                HttpMethod = httpMethod;
                Url = url;
            }

            public string ActionName { get; private set; }
            public string HttpMethod { get; private set; }
            public string Url { get; private set; }
        }
    }
}
