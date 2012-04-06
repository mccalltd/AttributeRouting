using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Framework {
    public abstract class RouteBuilder<TController, TParameter> : RouteBuilder<IRouteConstraint, TController, AttributeRoute<TController, TParameter>, TParameter, HttpContextBase, RouteData>
    {
        protected RouteBuilder(
            AttributeRoutingConfiguration<IRouteConstraint, TController, AttributeRoute<TController, TParameter>, TParameter, HttpContextBase, RouteData> configuration,
            IAttributeRouteFactory<IRouteConstraint, TController, AttributeRoute<TController, TParameter>, TParameter, HttpContextBase, RouteData> routeFactory,
            IConstraintFactory<IRouteConstraint> constraintFactory, IParameterFactory<TParameter> parameterFactory) 
            : base(configuration, routeFactory, constraintFactory, parameterFactory) {}
    }
}
