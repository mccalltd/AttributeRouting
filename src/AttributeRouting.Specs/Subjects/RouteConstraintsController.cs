using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RouteConstraintsController : Controller
    {
        [GET("Constraint/{p1}")]
        [RegexRouteConstraint("p1", @"\d+")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("MultipleConstraints/1/{p1}", RouteName = "MultipleConstraints1")]
        [GET("MultipleConstraints/2/{p1}", RouteName = "MultipleConstraints2")]
        [RegexRouteConstraint("p1", @"\d+", ForRouteNamed = "MultipleConstraints1")]
        [RegexRouteConstraint("p1", @"\d{4}", ForRouteNamed = "MultipleConstraints2")]
        public ActionResult MultipleRoutes()
        {
            return Content("");
        }
    }
}