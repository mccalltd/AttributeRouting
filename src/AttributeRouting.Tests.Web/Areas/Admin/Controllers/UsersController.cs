using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Areas.Admin.Controllers
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