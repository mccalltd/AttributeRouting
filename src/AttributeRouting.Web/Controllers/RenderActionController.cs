using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers
{
    [RoutePrefix("RenderAction")]
    public class RenderActionController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [POST("")]
        //[HttpPost]
        public ActionResult Index(object model)
        {
            return View();
        }

        [GET("Partial")]
        [POST("Partial")]
        //[HttpGet]
        //[HttpPost]
        [ChildActionOnly]
        public ActionResult Partial()
        {
            return View();
        }
    }
}
