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

        [GET("Optionals/{p1?}")]
        public ActionResult Optionals(string p1)
        {
            return Content("");
        }

        [GET("{controller}/{action}")]
        public string TheActionName()
        {
            return "is joe";
        }

        [GET("Optionals/In/Query?{x?}")]
        public string OptionalsInQuery()
        {
            return "optionals in query";
        }
    }
}