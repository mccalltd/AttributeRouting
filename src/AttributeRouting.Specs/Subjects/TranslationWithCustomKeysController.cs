using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RouteArea("Area", TranslationKey = "CustomAreaKey")]
    [RoutePrefix("Prefix", TranslationKey = "CustomPrefixKey")]
    public class TranslationWithCustomKeysController : Controller
    {
        [GET("Index", TranslationKey = "CustomRouteKey")]
        public ActionResult Index()
        {
            return Content("content");
        }
    }
}