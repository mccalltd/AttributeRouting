using System.Collections.Generic;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost.Framework.Factories
{
    internal class AttributeRouteFactory : IAttributeRouteFactory
    {
        private readonly HttpWebAttributeRoutingConfiguration _configuration;

        public AttributeRouteFactory(HttpWebAttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IAttributeRoute CreateAttributeRoute(string url,
                                                    IDictionary<string, object> defaults,
                                                    IDictionary<string, object> constraints,
                                                    IDictionary<string, object> dataTokens)
        {
            return new HttpWebAttributeRoute(url,
                                             new RouteValueDictionary(defaults),
                                             new RouteValueDictionary(constraints),
                                             new RouteValueDictionary(dataTokens),
                                             _configuration);
        }
    }
}
