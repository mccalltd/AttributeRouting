using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RestfulRouteConvention]
    public class RestfulRouteConventionWithExplicitRouteTestController : Controller
    {
        [GET("Legacy", IsAbsoluteUrl = true)]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}