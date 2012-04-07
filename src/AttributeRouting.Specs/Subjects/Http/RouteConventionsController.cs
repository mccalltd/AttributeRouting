using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http {

    [DefaultHttpRouteConvention]
    public class DefaultHttpRouteConventionController : ApiController {

        public string GetAll() {
            return "";
        }

        public string Get(int id) {
            return "";
        }

        public string Post() {
            return "";
        }

        public string Delete(int id) {
            return "";
        }

        public string Put(int id) {
            return "";
        }

        [GET("Custom")]
        public string Custom() {
            return "";
        }
    }

    [DefaultHttpRouteConvention]
    public class DefaultHttpRouteConventionWithExplicitRouteController : ApiController {
        [GET("Legacy", IsAbsoluteUrl = true)]
        public string Get() {
            return "";
        }
    }

    [DefaultHttpRouteConvention]
    public class DefaultHttpRouteConventionWithExplicitOrderedRouteController : ApiController {
        [GET("Primary", Order = 1)]
        public string Index() {
            return "";
        }
    }
}
