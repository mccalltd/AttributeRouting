using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttributeRouting
{
    public class AttributeRoute : Route
    {
        private readonly bool _useLowercaseRoutes;

        public AttributeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, bool useLowercaseRoutes) 
            : this(null, url, defaults, constraints, dataTokens, useLowercaseRoutes) {}

        public AttributeRoute(string name, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, bool useLowercaseRoutes) 
            : base(url, defaults, constraints, dataTokens, new MvcRouteHandler())
        {
            Name = name;
            _useLowercaseRoutes = useLowercaseRoutes;
        }

        public string Name { get; set; }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var virtualPath = base.GetVirtualPath(requestContext, values);

            if (!_useLowercaseRoutes)
                return virtualPath;

            if (virtualPath != null)
                virtualPath.VirtualPath = virtualPath.VirtualPath.ToLowerInvariant();

            return virtualPath;
        }
    }
}
