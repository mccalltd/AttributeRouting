using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    [RoutePrefix("RenderAction")]
    public class RenderActionController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [POST("")]
        public ActionResult Index(object model)
        {
            return View();
        }

        [Route("Partial")]
        public ActionResult Partial()
        {
            return View();
        }
    }
}
