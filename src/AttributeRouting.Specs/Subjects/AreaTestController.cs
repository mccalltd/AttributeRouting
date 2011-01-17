using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RouteArea("Area")]
    public class AreaTestController : Controller
    {
        [GET("Test")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("Area/DuplicateArea")]
        public ActionResult DuplicateArea()
        {
            return Content("");
        }
    }
}
