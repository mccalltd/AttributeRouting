using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RouteDefaultsController : Controller
    {
        [GET("InlineDefaults/{hello=sun}/{goodnight=moon}")]
        public ActionResult InlineDefaults()
        {
            return Content("");
        }

        [GET("Optionals/{p1?}/{?p2}/{?p3?}")]
        public ActionResult Optionals(string p1, string p2, string p3)
        {
            return Content("");
        }
    }
}