using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

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
    }
}
