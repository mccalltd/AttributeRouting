using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class StandardUsageController : Controller
    {
        [GET("", ActionPrecedence = 1)]
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

        [GET("Wildcards/{*pathInfo}")]
        public ActionResult Wildcards()
        {
            return Content("");
        }

        [Route("AnyVerb")]
        public ActionResult AnyVerb()
        {
            return Content("");
        }

        [GET]
        public ActionResult GetDefault()
        {
            return Content("");
        }

        [POST]
        public ActionResult PostDefault()
        {
            return Content("");
        }

        [PUT]
        public ActionResult PutDefault()
        {
            return Content("");
        }

        [DELETE]
        public ActionResult DeleteDefault()
        {
            return Content("");
        }

        [Route]
        public ActionResult RouteDefault()
        {
            return Content("");
        }
    }
}