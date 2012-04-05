using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace AttributeRouting.WebApi {
    public class GETAttribute : HttpRouteAttribute {
        public GETAttribute(string routeUrl) 
            : base(routeUrl, "GET", "HEAD") { }
    }
}
