using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Framework;

namespace AttributeRouting.Web.Mvc.Framework
{
    public class AttributeRouteContainer : AttributeRouteContainerBase<AttributeRoute<IController, UrlParameter>>
    {
        private readonly AttributeRoute _route;

        public AttributeRouteContainer(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens, 
            WebAttributeRoutingConfiguration<IController, UrlParameter> configuration)
        {
            _route = new AttributeRoute(url, defaults, constraints, dataTokens, configuration, this);
        }

        public override AttributeRoute<IController, UrlParameter> Route
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

        /// <summary>
        /// Defaults dictionary
        /// </summary>
        public override IDictionary<string, object> Defaults {
            get { return Route.Defaults; }
            set { Route.Defaults = new RouteValueDictionary(value); }
        }
    }
}
