using System.Net;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        [GET("", ActionPrecedence = 1)]
        [GET("Home", ActionPrecedence = 2)]
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

        [GET("About", ControllerPrecedence = 2, UseLowercaseRoute = false, AppendTrailingSlash = true)]
        public ActionResult About()
        {
            return Content("About");
        }

        [GET("Contact", ActionPrecedence = 2)]
        [GET("ContactUs", ActionPrecedence = 1, ControllerPrecedence = 1)]
        public ActionResult Contact()
        {
            return Content("Contact");
        }
    }
}
