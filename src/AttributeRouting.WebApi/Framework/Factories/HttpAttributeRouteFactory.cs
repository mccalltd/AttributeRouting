using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.AspNet.Framework.Factories;
using AttributeRouting.Framework;

namespace AttributeRouting.WebApi.Framework.Factories
{
    internal class HttpAttributeRouteFactory : AttributeRouteFactory<IHttpController, RouteParameter>
    {
        public override AttributeRouteContainerBase<AttributeRoute<IHttpController, RouteParameter>> CreateAttributeRoute(
            string url, 
            IDictionary<string, object> defaults, 
            IDictionary<string, object> constraints, 
            IDictionary<string, object> dataTokens,
            AttributeRoutingConfiguration<IRouteConstraint, IHttpController, AttributeRoute<IHttpController, RouteParameter>, RouteParameter, HttpContextBase, RouteData> configuration)
        {
            return new AttributeRouteContainer(url, 
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new RouteValueDictionary(dataTokens), configuration as HttpAttributeRoutingConfiguration);
        }
    }
}
