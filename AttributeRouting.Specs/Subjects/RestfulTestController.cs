using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RestfulTestController : Controller
    {
        [GET("Resources")]
        public ActionResult Index()
        {
            return Content("");
        }

        [POST("Resources")]
        public ActionResult Create()
        {
            return Content("");
        }

        [PUT("Resources/{id}")]
        public ActionResult Update(int id)
        {
            return Content("");
        }

        [DELETE("Resources/{id}")]
        public ActionResult Destroy(int id)
        {
            return Content("");
        }
    }
}
