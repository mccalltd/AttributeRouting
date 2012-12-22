using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects.Areas.Sample
{
    [RouteArea]
    public class DefaultRouteAreaController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}