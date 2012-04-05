using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using AttributeRouting.AspNet;
using AttributeRouting.AspNet.Framework;
using AttributeRouting.Framework;

namespace AttributeRouting.WebApi.Framework
{
    public class AttributeRouteContainer : AttributeRouteContainerBase<AttributeRoute<IHttpController, RouteParameter>>
    {
        private readonly AttributeRoute _route;

        public AttributeRouteContainer(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens,
            AspNetAttributeRoutingConfiguration<IHttpController, RouteParameter> configuration)
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
    }
}
