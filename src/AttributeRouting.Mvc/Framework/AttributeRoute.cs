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
    public class AttributeRoute : AttributeRoute<IController, UrlParameter>
    {
        public AttributeRoute(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens, 
            AspNetAttributeRoutingConfiguration<IController, UrlParameter> configuration,
            AttributeRouteContainerBase<AttributeRoute<IController, UrlParameter>> wrapper)
            : base(url, defaults, constraints, dataTokens, configuration, wrapper)
        {
        }
    }
}
