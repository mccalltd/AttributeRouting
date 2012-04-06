using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.WebHost;

namespace AttributeRouting.Web.Http.WebHost {
    public class HttpAttributeRoutingConfiguration : WebAttributeRoutingConfiguration<IHttpController, RouteParameter>
    {
        public HttpAttributeRoutingConfiguration()
            : base(() => HttpControllerRouteHandler.Instance)
        {
        }
    }
}
