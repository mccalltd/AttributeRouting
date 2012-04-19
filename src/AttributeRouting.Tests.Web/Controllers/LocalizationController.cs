using System.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
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
