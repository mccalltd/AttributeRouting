using System.Web.Mvc;
using AttributeRouting.Mvc;

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