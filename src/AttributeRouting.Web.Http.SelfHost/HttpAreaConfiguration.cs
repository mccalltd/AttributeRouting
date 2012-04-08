using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Web.Http.SelfHost.Framework;

namespace AttributeRouting.Web.Http.SelfHost
{
    public class HttpAreaConfiguration
        : AreaConfiguration<HttpRequestMessage, IHttpRouteData>
    {
        public HttpAreaConfiguration(string name, HttpAttributeRoutingConfiguration configuration) : base(name, configuration)
        {
        }
    }
}
