using System.Web.Mvc;
using AttributeRouting.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RouteArea("Users", Subdomain = "users")]
    public class SubdomainController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return Content("");
        }
    }

    [RouteArea("Admin", Subdomain = "private", AreaUrl = "admin")]
    public class SubdomainWithAreaUrlController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}
