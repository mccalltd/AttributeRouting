using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Mvc.Framework {
    public class RouteBuilder : RouteBuilder<IController, UrlParameter>
    {
        public RouteBuilder(AttributeRoutingConfiguration<IRouteConstraint, IController, AttributeRoute<IController, UrlParameter>, UrlParameter, HttpContextBase, RouteData> configuration, IAttributeRouteFactory<IRouteConstraint, IController, AttributeRoute<IController, UrlParameter>, UrlParameter, HttpContextBase, RouteData> routeFactory, IConstraintFactory<IRouteConstraint> constraintFactory, IParameterFactory<UrlParameter> parameterFactory) : base(configuration, routeFactory, constraintFactory, parameterFactory)
        {
        }
    }
}
