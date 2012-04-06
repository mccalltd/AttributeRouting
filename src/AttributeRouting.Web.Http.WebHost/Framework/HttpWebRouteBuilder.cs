using System.Web.Http;
using System.Web.Http.Controllers;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Framework;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost.Framework
{
    internal class HttpWebRouteBuilder : RouteBuilder<IHttpController, RouteParameter>
    {
        public HttpWebRouteBuilder(HttpAttributeRoutingConfiguration configuration, 
            HttpAttributeRouteFactory routeFactory,
            ConstraintFactory constraintFactory,
            IParameterFactory<RouteParameter> parameterFactory)
            : base(configuration, routeFactory, constraintFactory, parameterFactory)
        {
        }
    }
}
