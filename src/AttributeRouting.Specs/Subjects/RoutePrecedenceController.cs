using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RoutePrecedenceAmongRoutesController : Controller
    {
        [GET("Index/Second", ActionPrecedence = 2)]
        [GET("Index/Third", ActionPrecedence = 3)]
        [GET("Index/First", ActionPrecedence = 1)]
        [GET("Index/Fourth")]
        [GET("Index/Seventh", ActionPrecedence = -1)]
        [GET("Index/Fifth", ActionPrecedence = -3)]
        [GET("Index/Sixth", ActionPrecedence = -2)]
        public string Index()
        {
            return "";
        }    
    }

    public partial class RoutePrecedenceAmongActionsController : Controller
    {
        [GET("Route1", ControllerPrecedence = 1)]
        public string Route1() { return ""; }

        [GET("Route3", ControllerPrecedence = 3)]
        public string Route3() { return ""; }

        [GET("Route5", ControllerPrecedence = -3)]
        public string Route5() { return ""; }

        [GET("Route7", ControllerPrecedence = -1)]
        public string Route7() { return ""; }
    }

    public partial class RoutePrecedenceAmongActionsController
    {
        [GET("Route0", ControllerPrecedence = 0)]
        public string Route0() { return ""; }
        
        [GET("Route2", ControllerPrecedence = 2)]
        public string Route2() { return ""; }
        
        [GET("Route4")]
        public string Route4() { return ""; }

        [GET("Route6", ControllerPrecedence = -2)]
        public string Route6() { return ""; }
    }

    public class RoutePrecedenceAmongControllers3Controller : Controller
    {
        [GET("Controller3/Index")]
        public string Index() { return ""; }
    }

    public class RoutePrecedenceAmongControllers1Controller : Controller
    {
        [GET("Controller1/Index")]
        public string Index() { return ""; }
    }

    public class RoutePrecedenceAmongControllers2Controller : Controller
    {
        [GET("Controller2/Index")]
        public string Index() { return ""; }
    }

    public class RoutePrecedenceAmongControllers5Controller : Controller
    {
        [GET("Controller5/Index")]
        public string Index() { return ""; }
    }

    public class RoutePrecedenceAmongControllers4Controller : Controller
    {
        [GET("Controller4/Index")]
        public string Index() { return ""; }
    }

    public class RoutePrecedenceAmongTheSitesRoutesController : Controller
    {
        [GET("The-First-Route", SitePrecedence = 1)]
        public string TheFirstRoute() { return "yay!"; }

        [GET("The-Last-Route", SitePrecedence = -1)]
        public string TheLastRoute() { return "nay!"; }
    }

    public class RoutePrecedenceViaRoutePropertiesController : Controller
    {
        [GET("Route3", ActionPrecedence = 1)]
        [GET("Route5", ActionPrecedence = -1)]
        [GET("Route4")]
        public string RouteWhatever() { return ""; }

        [GET("Route1", SitePrecedence = 1)]
        public string Route1() { return ""; }

        [GET("Route7", SitePrecedence = -1)]
        public string Route7() { return ""; }
        
        [GET("Route2", ControllerPrecedence = 1)]
        public string Route2() { return ""; }

        [GET("Route6", ControllerPrecedence = -1)]
        public string Route6() { return ""; }
    }

    public abstract class RoutePrecedenceAmongDerivedControllersBaseController : Controller { }
    
    public class RoutePrecedenceAmongDerivedControllers1 : RoutePrecedenceAmongDerivedControllersBaseController
    {
        [GET("DerivedController1/Index")]
        public ActionResult Index()
        {
            return Content("");
        }
    }
    
    public class RoutePrecedenceAmongDerivedControllers2 : RoutePrecedenceAmongDerivedControllersBaseController
    {
        [GET("DerivedController2/Index")]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}