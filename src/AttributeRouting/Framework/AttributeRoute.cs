using System.Text.RegularExpressions;
using System.Web.Routing;

namespace AttributeRouting.Framework
{
    public class AttributeRoute : Route
    {
        private readonly bool _useLowercaseRoutes;
        private readonly bool _appendTrailingSlash;

        public AttributeRoute(
            string name,
            string url,
            RouteValueDictionary defaults,
            RouteValueDictionary constraints,
            RouteValueDictionary dataTokens,
            bool useLowercaseRoutes,
            bool appendTrailingSlash,
            IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            Name = name;
            _useLowercaseRoutes = useLowercaseRoutes;
            _appendTrailingSlash = appendTrailingSlash;
        }

        public string Name { get; set; }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var data = base.GetVirtualPath(requestContext, values);
            if (data != null)
                data.VirtualPath = GetFinalVirtualPath(data);

            return data;
        }

        private string GetFinalVirtualPath(VirtualPathData data)
        {
            var virtualPath = data.VirtualPath;

            // NOTE: Do not lowercase the querystring vals
            var match = Regex.Match(virtualPath, @"(?<path>[^\?]*)(?<query>\?.*)?");

            // Just covering my backside here in case the regex fails for some reason.
            if (!match.Success)
                return virtualPath.ToLowerInvariant();

            var path = match.Groups["path"].Value;
 
            if (_appendTrailingSlash && !path.EndsWith("/"))
                path += "/";
                
            if (_useLowercaseRoutes)
                path = path.ToLowerInvariant();
                
            return path + match.Groups["query"].Value;
        }
    }
}