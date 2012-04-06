using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Framework;

namespace AttributeRouting.Web.Mvc.Framework {
    public class RouteReflector
        : RouteReflector<IRouteConstraint, IController, AttributeRoute<IController, UrlParameter>, UrlParameter, HttpContextBase, RouteData>
    {
        public RouteReflector(AspNetAttributeRoutingConfiguration<IController, UrlParameter> configuration) 
            : base(configuration) { }
    }
}
