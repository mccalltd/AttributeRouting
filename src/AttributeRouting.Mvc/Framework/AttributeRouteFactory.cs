using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Mvc.Framework {
    public class AttributeRouteFactory : IAttributeRouteFactory<IRouteConstraint, IController, MvcRoute, UrlParameter> {
        /// <summary>
        /// Create a new attribute route that wraps an underlying framework route
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public AttributeRouteBase<MvcRoute> CreateAttributeRoute(
            string url, 
            IDictionary<string, object> defaults, 
            IDictionary<string, object> constraints, 
            IDictionary<string, object> dataTokens,
            AttributeRoutingConfiguration<IRouteConstraint, IController, MvcRoute, UrlParameter> configuration) {
                return new AttributeRoute(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new RouteValueDictionary(dataTokens), configuration as AttributeRoutingConfiguration);
        }
    }
}
