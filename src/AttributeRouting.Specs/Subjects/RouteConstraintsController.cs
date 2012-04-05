using System.Web.Mvc;
using AttributeRouting.Mvc;

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

        [GET(@"InlineConstraints/{number(\d+)}/{word(\w{2})}/{alphanum([A-Za-z0-9]*)}/{capture((gotcha))}")]
        public ActionResult InlineConstraints(long number, string word)
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