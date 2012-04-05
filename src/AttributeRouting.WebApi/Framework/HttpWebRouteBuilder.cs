using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.AspNet.Framework.Factories;
using AttributeRouting.Framework.Factories;
using AttributeRouting.WebApi.Framework.Factories;

namespace AttributeRouting.WebApi.Framework
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
