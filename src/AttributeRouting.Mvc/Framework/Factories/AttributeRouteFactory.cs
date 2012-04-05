using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Mvc.Framework.Factories {
    public class AttributeRouteFactory : IAttributeRouteFactory<IRouteConstraint, IController, AttributeRoute, UrlParameter> {
        /// <summary>
        /// Create a new attribute route that wraps an underlying framework route
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public AttributeRouteContainerBase<AttributeRoute> CreateAttributeRoute(
            string url, 
            IDictionary<string, object> defaults, 
            IDictionary<string, object> constraints, 
            IDictionary<string, object> dataTokens,
            AttributeRoutingConfiguration<IRouteConstraint, IController, AttributeRoute, UrlParameter> configuration) {
                return new AttributeRouteContainer(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new RouteValueDictionary(dataTokens), configuration as AttributeRoutingConfiguration);
        }
    }
}
