using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public partial class ActionOrderTestController : Controller
    {
        [GET("Route1")]
        [RouteActionOrder(1)]
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

    public partial class ActionOrderTestController
    {
        [GET("Route2")]
        [RouteActionOrder(2)]
        public ActionResult Route2()
        {
            return Content("");
        }
    }
}
