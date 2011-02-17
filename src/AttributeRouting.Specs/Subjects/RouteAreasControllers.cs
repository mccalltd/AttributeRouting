using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RouteArea("Area")]
    public class AreasController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("Area/DuplicatePrefix")]
        public ActionResult DuplicatePrefix()
        {
            return Content("");
        }

        [GET("AreaAbsolute", IsAbsoluteUrl = true)]
        public ActionResult Absolute()
        {
            return Content("");
        }
    }

    [RouteArea("Area", AreaUrl = "ExplicitArea")]
    public class ExplicitAreaUrlController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("ExplicitArea/DuplicatePrefix")]
        public ActionResult DuplicatePrefix()
        {
            return Content("");
        }
    }
}
