using System.Web.Mvc;
using AttributeRouting.Mvc;

namespace AttributeRouting.Web.Controllers
{
    [RouteArea("Localization", AreaUrl = "{culture}/Localization")]
    [RoutePrefix("Prefix")]
    public class LocalizationController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
