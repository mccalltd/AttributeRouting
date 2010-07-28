using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Controllers
{
    [RouteArea("Area")]
    public class AreaTestController : Controller
    {
        [GET("Test")]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}
