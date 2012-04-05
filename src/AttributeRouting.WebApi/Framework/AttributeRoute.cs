using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using AttributeRouting.AspNet;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.Framework;

namespace AttributeRouting.Http.WebHost.Framework
{
    public class AttributeRoute : AttributeRoute<IHttpController, RouteParameter>
    {
        public AttributeRoute(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens,
            AspNetAttributeRoutingConfiguration<IHttpController, RouteParameter> configuration,
            AttributeRouteContainerBase<AttributeRoute<IHttpController, RouteParameter>> wrapper)
            : base(url, defaults, constraints, dataTokens, configuration, wrapper)
        {
        }
    }
}
