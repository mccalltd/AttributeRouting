using System.Web.Http;
using System.Web.Http.Controllers;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Http.WebHost.Framework.Factories;
using AttributeRouting.Web.Framework;
using AttributeRouting.Web.Framework.Factories;

namespace AttributeRouting.Http.WebHost.Framework
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
