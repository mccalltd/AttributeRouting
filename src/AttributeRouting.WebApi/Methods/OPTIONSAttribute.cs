using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AttributeRouting.WebApi {
    public class OPTIONSAttribute : HttpRouteAttribute {
        public OPTIONSAttribute(string routeUrl)
            : base(routeUrl, "OPTIONS")
        {
            
        }
    }
}
