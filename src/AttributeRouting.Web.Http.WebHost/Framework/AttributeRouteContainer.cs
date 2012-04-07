using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Framework;

namespace AttributeRouting.Web.Http.WebHost.Framework
{
    public class AttributeRouteContainer : AttributeRouteContainerBase<AttributeRoute<IHttpController, RouteParameter>>
    {
        private readonly AttributeRoute _route;

        public AttributeRouteContainer(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens,
            WebAttributeRoutingConfiguration<IHttpController, RouteParameter> configuration)
        {
            _route = new AttributeRoute(url, defaults, constraints, dataTokens, configuration, this);
        }

        public override AttributeRoute<IHttpController, RouteParameter> Route
        {
            get { return _route; }
        }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        public override IDictionary<string, object> DataTokens
        {
            get { return Route.DataTokens; }
            set { Route.DataTokens = new RouteValueDictionary(value); }
        }

        /// <summary>
        /// Constraints dictionary
        /// </summary>
        public override IDictionary<string, object> Constraints {
            get { return Route.Constraints; }
            set { Route.Constraints = new RouteValueDictionary(value); }
        }
    }
}
