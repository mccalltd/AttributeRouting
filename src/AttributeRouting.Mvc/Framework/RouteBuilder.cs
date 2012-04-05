using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Mvc.Framework {
    internal class RouteBuilder : RouteBuilder<IRouteConstraint, IController, MvcRoute, UrlParameter> {
        public RouteBuilder(
            AttributeRoutingConfiguration<IRouteConstraint, IController, MvcRoute, UrlParameter> configuration, 
            IAttributeRouteFactory<IRouteConstraint, IController, MvcRoute, UrlParameter> routeFactory, 
            IConstraintFactory<IRouteConstraint> constraintFactory, IParameterFactory<UrlParameter> parameterFactory) 
            : base(configuration, routeFactory, constraintFactory, parameterFactory) {}
    }
}
