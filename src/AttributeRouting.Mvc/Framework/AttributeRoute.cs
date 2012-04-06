using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Framework;

namespace AttributeRouting.Web.Mvc.Framework
{
    public class AttributeRoute : AttributeRoute<IController, UrlParameter>
    {
        public AttributeRoute(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens, 
            AspNetAttributeRoutingConfiguration<IController, UrlParameter> configuration,
            AttributeRouteContainerBase<AttributeRoute<IController, UrlParameter>> wrapper)
            : base(url, defaults, constraints, dataTokens, configuration, wrapper)
        {
        }
    }
}
