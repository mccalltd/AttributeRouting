using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Http.SelfHost.Framework;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost {
    public class HttpAttributeRoutingConfiguration : AttributeRoutingConfiguration<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter, HttpRequestMessage, IHttpRouteData>
    {
        public HttpAttributeRoutingConfiguration() 
            : base(new AttributeRouteFactory(), new HttpRouteConstraintFactory(), new RouteParameterFactory()) {}
    }
}
