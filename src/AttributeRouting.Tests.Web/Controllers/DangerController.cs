using System.Web.Mvc;
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
        [ValidateInput(false)]
        public ActionResult Index_Post(string badstuff)
        {
            return Content("You survived the danger!");
        }
    }
}
