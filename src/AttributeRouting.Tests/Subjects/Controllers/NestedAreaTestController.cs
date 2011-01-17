using System.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Controllers
{
    [RouteArea("Area")]
    [RoutePrefix("Parent/{parentId}")]
    public class NestedAreaTestController : Controller
    {
        [GET("Child")]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}
