using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class StandardUsageController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("");
        }

        [POST("Create")]
        public ActionResult Create()
        {
            return Content("");
        }

        [PUT("Update/{id}")]
        public ActionResult Update()
        {
            return Content("");
        }

        [DELETE("Destroy/{id}")]
        public ActionResult Destroy()
        {
            return Content("");
        }
    }
}