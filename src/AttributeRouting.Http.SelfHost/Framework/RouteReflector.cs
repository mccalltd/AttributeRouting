using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Http.SelfHost.Framework {
    public class RouteReflector : RouteReflector<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter, HttpRequestMessage, IHttpRouteData>
    {
        public RouteReflector(HttpAttributeRoutingConfiguration configuration) : base(configuration) {}
    }
}
