using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers
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
