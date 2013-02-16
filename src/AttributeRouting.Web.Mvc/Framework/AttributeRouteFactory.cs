using System.Collections.Generic;
using System.Web.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Web.Mvc.Framework
{
    internal class AttributeRouteFactory : IAttributeRouteFactory
    {
        private readonly Configuration _configuration;

        public AttributeRouteFactory(Configuration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<IAttributeRoute> CreateAttributeRoutes(string url, IDictionary<string, object> defaults, IDictionary<string, object> constraints, IDictionary<string, object> dataTokens)
        {
            yield return new AttributeRoute(url,
                                            new RouteValueDictionary(defaults),
                                            new RouteValueDictionary(constraints),
                                            new RouteValueDictionary(dataTokens),
                                            _configuration);
        }
    }
}
