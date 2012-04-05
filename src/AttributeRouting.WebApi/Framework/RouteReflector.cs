using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.WebApi.Framework {
    public class RouteReflector : RouteReflector<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter, HttpRequestMessage, IHttpRouteData>
    {
        public RouteReflector(AttributeRoutingConfiguration configuration) : base(configuration) {}
    }
}
