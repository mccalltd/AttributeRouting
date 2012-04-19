using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("Trailing-Slash")]
    public class TrailingSlashesController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("Route-Override-True", AppendTrailingSlash = true)]
        public ActionResult RouteOverrideTrue()
        {
            return Content("");
        }

        [GET("Route-Override-False", AppendTrailingSlash = false)]
        public ActionResult RouteOverrideFalse()
        {
            return Content("");
        }
    }
}