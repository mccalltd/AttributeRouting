using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Controllers
{
    public class RestfulTestController : Controller
    {
        [GET("Resources")]
        public ActionResult Index()
        {
            return Content("");
        }

        [PUT("Resources")]
        public ActionResult Create()
        {
            return Content("");
        }

        [POST("Resources/{id}")]
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
