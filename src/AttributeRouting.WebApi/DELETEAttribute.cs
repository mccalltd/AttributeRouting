using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AttributeRouting.WebApi {
    public class DELETEAttribute : HttpRouteAttribute {
        public DELETEAttribute(string routeUrl)
            : base(routeUrl, "DELETE")
        {
            
        }
    }
}
