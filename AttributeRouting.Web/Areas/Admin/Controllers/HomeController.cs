using System.Web.Mvc;

namespace AttributeRouting.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminControllerBase
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}