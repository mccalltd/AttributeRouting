using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Controllers
{
    public class TestController : Controller
    {
        [GET("Test/Optionals/{?p1}/{?p2}/{?p3}")]
        public ActionResult OptionalParametersWithAToken()
        {
            return Content("");
        }

        [GET("Test/Multiple", Order = 1)]
        [GET("Test/Multiple/Routes", Order = 2)]
        [GET("Test/Multiple/Routes/Again", Order = 3)]
        public ActionResult MultipleRoutes()
        {
            return Content("");
        }

        [GET("Test/Default/{param1}")]
        [RouteDefault("param1", "mapleleaf")]
        public ActionResult Default(string param1)
        {
            return Content("");
        }

        [GET("Test/Constraint/{cat}")]
        [RegexRouteConstraint("cat", @"^(kitty|meow-meow|purrbot)$")]
        public ActionResult Constraint(string cat)
        {
            return Content("");
        }

        [GET("Test/Medley/First/{number}", Order = 1, RouteName = "FirstDitty")]
        [RouteDefault("number", 666, ForRouteNamed = "FirstDitty")]
        [RegexRouteConstraint("number", @"^\d{4}$", ForRouteNamed = "FirstDitty")]
        [GET("Test/Medley/Second/{number}", Order = 2, RouteName = "SecondDitty")]
        [RouteDefault("number", 777, ForRouteNamed = "SecondDitty")]
        [RegexRouteConstraint("number", @"^\d{1}$", ForRouteNamed = "SecondDitty")]
        public ActionResult MultipleRoutesWithDefaultsAndConstraints()
        {
            return Content("");
        }
    }
}
