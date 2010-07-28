using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        [GET("", Order = 1)]
        [GET("Home", Order = 2)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileNotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return Content("<h1>404</h1>You got this because the route is not mapped.");
        }
    }
}
