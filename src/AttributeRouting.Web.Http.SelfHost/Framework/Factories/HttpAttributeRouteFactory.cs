using System.Collections.Generic;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost.Framework.Factories
{
    public class HttpAttributeRouteFactory : IAttributeRouteFactory
    {
        private readonly HttpAttributeRoutingConfiguration _configuration;

        public HttpAttributeRouteFactory(HttpAttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Create a new attribute route that wraps an underlying framework route
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <returns></returns>
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
