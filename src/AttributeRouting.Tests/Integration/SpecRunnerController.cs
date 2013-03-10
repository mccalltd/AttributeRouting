using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Integration
{
    public class SpecRunnerController : Controller
    {
        [GET("", SitePrecedence = 1)]
        public ActionResult Index()
        {
            return View("~/Integration/SpecRunner.cshtml");
        }
    }
}
