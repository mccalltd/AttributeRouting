using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public partial class RoutePrecedenceTestController : Controller
    {
        [GET("Route1", Precedence = 1)]
        public ActionResult Route1()
        {
            return Content("");
        }

        [GET("Route3")]
        public ActionResult Route3()
        {
            return Content("");
        }
    }

    public partial class RoutePrecedenceTestController
    {
        [GET("Route2", Precedence = 2)]
        public ActionResult Route2()
        {
            return Content("");
        }
    }
}
