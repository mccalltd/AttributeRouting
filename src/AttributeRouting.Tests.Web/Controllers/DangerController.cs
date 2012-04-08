using System.Web.Mvc;
using AttributeRouting.Web;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    [RoutePrefix("Danger")]
    public class DangerController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [POST("")]
        public ActionResult Index_Post()
        {
            return Content("You survived the danger!");
        }
    }
}
