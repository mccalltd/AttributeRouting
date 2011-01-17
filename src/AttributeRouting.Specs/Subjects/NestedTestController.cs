using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("Parent/{parentId}")]
    public class NestedTestController : Controller
    {
        [GET("Child")]
        public ActionResult Index(int parentId)
        {
            return Content("");
        }

        [GET("Parent/{parentId}/DuplicateNest")]
        public ActionResult DuplicateNest()
        {
            return Content("");
        }
    }
}
