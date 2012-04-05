using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AttributeRouting.Http.WebHost
{
    public class TRACEAttribute : HttpRouteAttribute {
        public TRACEAttribute(string routeUrl)
            : base(routeUrl, "TRACE")
        {
            
        }
    }
}
