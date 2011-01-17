using System.Web.Mvc;

namespace AttributeRouting.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminControllerBase
    {
        [GET("Users")]
        public ActionResult Index()
        {
            return View();
        }
    }
}