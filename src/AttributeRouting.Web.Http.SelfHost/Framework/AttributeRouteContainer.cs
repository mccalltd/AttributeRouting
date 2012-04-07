using System;
using System.Collections.Generic;
using System.Web.Http.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Web.Http.SelfHost.Framework {
    /// <summary>
    /// Route supporting the AttributeRouting framework.
    /// </summary>
    public class AttributeRouteContainer : AttributeRouteContainerBase<AttributeRoute> {
        private readonly HttpAttributeRoutingConfiguration _configuration;
        private readonly AttributeRoute _route;

        /// <summary>
        /// Route supporting the AttributeRouting framework.
        /// </summary>
        public AttributeRouteContainer(
            string url,
            HttpRouteValueDictionary defaults,
            HttpRouteValueDictionary constraints,
            HttpRouteValueDictionary dataTokens,
            HttpAttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
            _route = new AttributeRoute(url, defaults, constraints, dataTokens, configuration, this);
        }

        public override AttributeRoute Route {
            get { return _route; }
        }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        public override IDictionary<string, object> DataTokens {
            get { return Route.DataTokens; }
            set { throw new NotImplementedException("WebAPI HttpRoute.DataTokens has no setter."); }
        }

        /// <summary>
        /// Constraints dictionary
        /// </summary>
        public override IDictionary<string, object> Constraints {
            get { return Route.Constraints; }
            set { throw new NotImplementedException("WebAPI HttpRoute.Constraints has no setter."); }
        }
    }
}
