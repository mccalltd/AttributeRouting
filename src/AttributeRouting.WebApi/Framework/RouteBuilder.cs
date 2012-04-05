using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.WebApi.Framework {
    internal class RouteBuilder : RouteBuilder<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter> {
        public RouteBuilder(
            AttributeRoutingConfiguration<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter> configuration,
            IAttributeRouteFactory<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter> routeFactory,
            IConstraintFactory<IHttpRouteConstraint> constraintFactory, IParameterFactory<RouteParameter> parameterFactory) 
            : base(configuration, routeFactory, constraintFactory, parameterFactory) {}
    }
}
