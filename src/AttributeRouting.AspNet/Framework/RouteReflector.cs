using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.AspNet.Framework {
    internal class RouteReflector<TController, TParameter> 
        : RouteReflector<IRouteConstraint, TController, AttributeRoute<TController, TParameter>, TParameter, HttpContextBase, RouteData>
    {
        public RouteReflector(AspNetAttributeRoutingConfiguration<TController, TParameter> configuration) 
            : base(configuration) { }
    }
}
