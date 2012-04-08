using System.Web.Mvc;
using AttributeRouting.Web;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected void Flash(string message)
        {
            TempData["flash"] = message;
        }

        [GET("BaseTestMethod")]
        public JsonResult BaseTestMethod()
        {
            return Json(new
            {
                Test = "Hello World"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}