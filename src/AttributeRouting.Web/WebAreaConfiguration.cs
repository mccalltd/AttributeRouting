using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Web.Framework;

namespace AttributeRouting.Web
{
    public class WebAreaConfiguration<TController, TParameter>
        : AreaConfiguration<TController, AttributeRoute<TController, TParameter>, TParameter, HttpContextBase, RouteData>
    {
        public WebAreaConfiguration(string name, 
            WebAttributeRoutingConfiguration<TController, TParameter> configuration) : base(name, configuration)
        {
        }
    }
}
