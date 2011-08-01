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
        public ActionResult Index(object model)
        {
            return View();
        }

        [Route("Partial")]
        public ActionResult Partial()
        {
            return View();
        }
    }
}
