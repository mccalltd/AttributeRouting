using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers
{
    [RoutePrefix("BadStuff")]
    public class BadStuffController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [POST("")]
        public ActionResult Index(string differentiator)
        {
            return Content("Made it through!");
        }
    }
}
