using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace AttributeRouting.WebApi {
    public class PUTAttribute : HttpRouteAttribute {
        public PUTAttribute(string routeUrl) 
            : base(routeUrl, "PUT") { }
    }
}
