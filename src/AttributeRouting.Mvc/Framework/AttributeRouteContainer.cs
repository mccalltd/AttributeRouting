using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Mvc.Framework {
    /// <summary>
    /// Route supporting the AttributeRouting framework.
    /// </summary>
    public class AttributeRouteContainer : AttributeRouteContainerBase<AttributeRoute> {
        private readonly AttributeRoutingConfiguration _configuration;
        private readonly AttributeRoute _route;

        /// <summary>
        /// Route supporting the AttributeRouting framework.
        /// </summary>
        public AttributeRouteContainer(
            string url,
            RouteValueDictionary defaults,
            RouteValueDictionary constraints,
            RouteValueDictionary dataTokens,
            AttributeRoutingConfiguration configuration) {
            _configuration = configuration;
            _route = new AttributeRoute(url, defaults, constraints, dataTokens, configuration, configuration.RouteHandlerFactory(), this);
        }

        public override AttributeRoute Route {
            get { return _route; }
        }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        public override IDictionary<string, object> DataTokens {
            get { return Route.DataTokens; }
            set { Route.DataTokens = new RouteValueDictionary(value); }
        }
    }
}
