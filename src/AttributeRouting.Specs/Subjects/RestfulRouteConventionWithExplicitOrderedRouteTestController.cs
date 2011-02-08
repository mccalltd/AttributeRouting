using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RestfulRouteConvention]
    public class RestfulRouteConventionWithExplicitOrderedRouteTestController : Controller
    {
        [GET("Primary", Order = 1)]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}