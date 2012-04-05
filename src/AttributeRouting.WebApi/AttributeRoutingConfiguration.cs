using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.WebApi.Framework;

namespace AttributeRouting.WebApi {
    public class AttributeRoutingConfiguration : AttributeRoutingConfiguration<IHttpRouteConstraint, IHttpController, AttributeRoute, RouteParameter> {

        public AttributeRoutingConfiguration()
        {
        }

        /// <summary>
        /// Adds all the routes for the specified controller type to the end of the route collection.
        /// </summary>
        /// <typeparam name="TController">The controller type</typeparam>
        public void AddRoutesFromController<TController>()
            where TController : IHttpController {
            AddRoutesFromController(typeof(TController));
        }
    }
}
