using System.Linq;
using System.Web.Routing;

namespace AttributeRouting.Framework
{
    public class AttributeRoute : Route
    {
        private readonly bool _useLowercaseRoutes;

        public AttributeRoute(
            string name, 
            string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints,
            RouteValueDictionary dataTokens, 
            bool useLowercaseRoutes, 
            IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            Name = name;
            _useLowercaseRoutes = useLowercaseRoutes;
        }

        public string Name { get; set; }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var data = base.GetVirtualPath(requestContext, values);

            if (_useLowercaseRoutes && data != null)
                data.VirtualPath = data.VirtualPath.ToLowerInvariant();

            return data;
        }
    }
}