using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RouteDefaultsController : Controller
    {
        [GET("Default/{p1}")]
        [RouteDefault("p1", "variable")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("InlineDefaults/{hello=sun}/{goodnight=moon}")]
        public ActionResult InlineDefaults()
        {
            return Content("");
        }

        [GET("Optionals/{?p1}/{?p2}")]
        public ActionResult Optionals()
        {
            return Content("");
        }

        [GET("MultipleDefaults/1/{p1}", RouteName = "MultipleDefaults1")]
        [GET("MultipleDefaults/2/{p1}", RouteName = "MultipleDefaults2")]
        [RouteDefault("p1", "first", ForRouteNamed = "MultipleDefaults1")]
        [RouteDefault("p1", "second", ForRouteNamed = "MultipleDefaults2")]
        public ActionResult MultipleRoutes()
        {
            return Content("");
        }
    }
}