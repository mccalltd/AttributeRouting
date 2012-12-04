using System.Collections.Generic;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Http.Framework;

namespace AttributeRouting.Web.Http.SelfHost.Framework.Factories
{
    internal class AttributeRouteFactory : IAttributeRouteFactory
    {
        private readonly HttpAttributeRoutingConfiguration _configuration;

        public AttributeRouteFactory(HttpAttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<IAttributeRoute> CreateAttributeRoutes(string url, IDictionary<string, object> defaults, IDictionary<string, object> constraints, IDictionary<string, object> dataTokens)
        {
            yield return new HttpAttributeRoute(url,
                                                new HttpRouteValueDictionary(defaults),
                                                new HttpRouteValueDictionary(constraints),
                                                new HttpRouteValueDictionary(dataTokens),
                                                _configuration);
        }
    }
}
