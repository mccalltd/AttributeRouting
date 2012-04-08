using System.Web.Mvc;
using AttributeRouting.Web;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Areas.Admin.Controllers
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