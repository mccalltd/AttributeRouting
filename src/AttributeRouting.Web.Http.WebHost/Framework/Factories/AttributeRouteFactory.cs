using System.Collections.Generic;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Http.Framework;

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
            return new HttpAttributeRoute(url,
                                          new HttpRouteValueDictionary(defaults),
                                          new HttpRouteValueDictionary(constraints),
                                          new HttpRouteValueDictionary(dataTokens),
                                          _configuration);
        }
    }
}
