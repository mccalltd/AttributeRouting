using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Framework;
using AttributeRouting.Web.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost.Framework.Factories
{
    internal class HttpAttributeRouteFactory : IAttributeRouteFactory
    {
        private readonly HttpAttributeRoutingConfiguration _configuration;

        public HttpAttributeRouteFactory(HttpAttributeRoutingConfiguration configuration) {
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
        public IAttributeRoute CreateAttributeRoute(string url, IDictionary<string, object> defaults, IDictionary<string, object> constraints, IDictionary<string, object> dataTokens) {
            return new AttributeRoute<RouteParameter>(url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new RouteValueDictionary(dataTokens), 
                _configuration);
        }
    }
}
