using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.AspNet.Framework.Factories;
using AttributeRouting.Framework;

namespace AttributeRouting.Mvc.Framework.Factories {
    internal class MvcAttributeRouteFactory : AttributeRouteFactory<IController, UrlParameter>
    {
        public override AttributeRouteContainerBase<AttributeRoute<IController, UrlParameter>> CreateAttributeRoute(
            string url, 
            IDictionary<string, object> defaults, 
            IDictionary<string, object> constraints, 
            IDictionary<string, object> dataTokens, 
            AttributeRoutingConfiguration<IRouteConstraint, IController, AttributeRoute<IController, UrlParameter>, UrlParameter, HttpContextBase, RouteData> configuration)
        {
            return new AttributeRouteContainer(url,
                    new RouteValueDictionary(defaults),
                    new RouteValueDictionary(constraints),
                    new RouteValueDictionary(dataTokens),
                    configuration as AttributeRoutingConfiguration);
        }

    }
}
