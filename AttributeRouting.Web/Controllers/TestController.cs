using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers
{
    public class TestController : Controller
    {
        [GET("test/index")]
        public ActionResult Index()
        {
            return Json(new { controller = "TestController", action = "Index" },
                        JsonRequestBehavior.AllowGet);
        }

        [GET("test/parametersshouldmap/{param1}/{param2}")]
        public ActionResult ParametersShouldMap(string param1, string param2)
        {
            return Json(new { controller = "TestController", action = "ParametersShouldMap", param1, param2 },
                        JsonRequestBehavior.AllowGet);
        }

        [GET("test/throwerror")]
        public ActionResult ThrowError()
        {
            throw new ApplicationException("This is an example error");
        }
    }
}
