using System.Web.Mvc;
using AttributeRouting.Web;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("Inline-Constraints")]
    public class InlineRouteConstraintsController : Controller
    {
        [GET("Int/{x:int}")]
        public string Int(int x)
        {
            return "";
        }

        [GET("Long/{x:long}")]
        public string Long(long x)
        {
            return "";
        }

        [GET("Float/{x:float}")]
        public string Float(float x)
        {
            return "";
        }

        [GET("Double/{x:double}")]
        public string Double(double x)
        {
            return "";
        }

        [GET("Decimal/{x:decimal:min(23)}")]
        public string Decimal(decimal x)
        {
            return "";
        }

        [GET("Bool/{x:bool}")]
        public string Bool(bool x)
        {
            return "";
        }
    }

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