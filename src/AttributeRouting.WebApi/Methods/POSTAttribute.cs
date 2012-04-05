using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AttributeRouting.Http.WebHost
{
    public class POSTAttribute : HttpRouteAttribute {
        public POSTAttribute(string routeUrl)
            : base(routeUrl, "POST")
        {
            
        }
    }
}
