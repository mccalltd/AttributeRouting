using System.Web.Mvc;
using AttributeRouting.Mvc;

namespace AttributeRouting.Web.Controllers
{
    [RoutePrefix("Resources/{resourceId}")]
    public class NestedController : ControllerBase
    {
        [GET("Nested")]
        public ActionResult Index(int resourceId)
        {
            return View();
        }

        [GET("Nested/{id}")]
        public ActionResult Show(int resourceId, int id)
        {
            return View();
        }
    }
}