using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.AspNet;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.Framework;

namespace AttributeRouting.Mvc.Framework {
    public class RouteReflector
        : RouteReflector<IRouteConstraint, IController, AttributeRoute<IController, UrlParameter>, UrlParameter, HttpContextBase, RouteData>
    {
        public RouteReflector(AspNetAttributeRoutingConfiguration<IController, UrlParameter> configuration) 
            : base(configuration) { }
    }
}
