using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RouteArea("CustomArea", TranslationKey = "CustomAreaKey")]
    [RoutePrefix("CustomPrefix", TranslationKey = "CustomPrefixKey")]
    public class TranslationWithCustomKeysController : Controller
    {
        [GET("CustomIndex", TranslationKey = "CustomRouteKey")]
        public ActionResult CustomIndex()
        {
            return Content("content");
        }
    }
}