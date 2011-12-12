using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttributeRouting.Framework
{
    public class AttributeRoute : Route
    {
        private readonly bool _useLowercaseRoutes;
        private readonly bool _preserveCaseForRouteParameters;

        public AttributeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints,
                              RouteValueDictionary dataTokens, bool useLowercaseRoutes, bool preservepreserveCaseForRouteParameters)
            : this(null, url, defaults, constraints, dataTokens, useLowercaseRoutes, preservepreserveCaseForRouteParameters) {}

        public AttributeRoute(string name, string url, RouteValueDictionary defaults, RouteValueDictionary constraints,
                              RouteValueDictionary dataTokens, bool useLowercaseRoutes, bool preserveCaseForRouteParameters)
            : base(url, defaults, constraints, dataTokens, new MvcRouteHandler())
        {
            Name = name;
            _useLowercaseRoutes = useLowercaseRoutes;
            _preserveCaseForRouteParameters = preserveCaseForRouteParameters;
        }

        public string Name { get; set; }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (!_useLowercaseRoutes)
                return base.GetVirtualPath(requestContext, values);

            var lowercaseValues = GetLowercaseRouteValues(values);

            return base.GetVirtualPath(requestContext, lowercaseValues);
        }

        public RouteValueDictionary GetLowercaseRouteValues(RouteValueDictionary values)
        {
            // Perform some lowercasing:
            var lowercaseValues = new RouteValueDictionary();
            var mvcSegments = new[] { "action", "controller", "area" };

            foreach (var keyValuePair in values)
            {
                var key = keyValuePair.Key;
                object value;

                if (mvcSegments.Contains(key.ToLowerInvariant()) || !_preserveCaseForRouteParameters)
                    value = ((string)keyValuePair.Value).ToLowerInvariant();
                else
                    value = keyValuePair.Value;

                lowercaseValues.Add(key, value);
            }

            return lowercaseValues;
        }
    }
}