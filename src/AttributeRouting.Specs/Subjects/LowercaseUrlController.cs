using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("LowercaseUrl")]
    public class LowercaseUrlController : Controller
    {
        [GET("Hello/{userName}/Goodbye")]
        public ActionResult Index(string userName)
        {
            return Content("How ya doing today?");
        }

        [GET("Lowercase-Override/{routeParam}", UseLowercaseRoute = true)]
        public ActionResult LowercaseOverride()
        {
            return Content("");
        }

        [GET("Lowercase-Preserve-Url-Param-Case-Override/{routeParam}", UseLowercaseRoute = true, PreserveCaseForUrlParameters = true)]
        public ActionResult LowercasePreserveUrlParamCaseOverride()
        {
            return Content("");
        }
    }
}
