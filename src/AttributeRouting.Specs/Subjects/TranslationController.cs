using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RouteArea("Area")]
    [RoutePrefix("Prefix")]
    public class TranslationController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("content");
        }
    }
}
