using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.AspNet;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.Framework;

namespace AttributeRouting.Mvc.Framework
{
    public class AttributeRouteContainer : AttributeRouteContainerBase<AttributeRoute<IController, UrlParameter>>
    {
        private readonly AttributeRoute _route;

        public AttributeRouteContainer(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens, 
            AspNetAttributeRoutingConfiguration<IController, UrlParameter> configuration)
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
    }
}
