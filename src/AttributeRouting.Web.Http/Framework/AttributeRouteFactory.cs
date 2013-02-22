using System.Collections.Generic;
using System.Web.Http.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Web.Http.Framework
{
    internal class AttributeRouteFactory : IAttributeRouteFactory
    {
        private readonly HttpConfigurationBase _configuration;

        public AttributeRouteFactory(HttpConfigurationBase configuration)
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
