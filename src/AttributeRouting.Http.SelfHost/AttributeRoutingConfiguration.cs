using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Http.SelfHost.Framework;

namespace AttributeRouting.Http.SelfHost {
    public class HttpAttributeRoutingConfiguration : AttributeRoutingConfiguration<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter, HttpRequestMessage, IHttpRouteData>
    {
    }
}
