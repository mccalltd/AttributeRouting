using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.WebHost;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost {
    public class HttpAttributeRoutingConfiguration : WebAttributeRoutingConfiguration<IHttpController, RouteParameter>
    {
        public HttpAttributeRoutingConfiguration()
            : base(() => HttpControllerRouteHandler.Instance, new HttpAttributeRouteFactory(), new ConstraintFactory(), new RouteParameterFactory())
        {
        }
    }
}
