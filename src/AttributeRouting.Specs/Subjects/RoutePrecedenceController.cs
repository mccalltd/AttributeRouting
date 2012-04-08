using System.Web.Mvc;
using AttributeRouting.Web;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RoutePrecedenceAmongRoutesController : Controller
    {
        [GET("Index/Second", Order = 2)]
        [GET("Index/Third", Order = 3)]
        [GET("Index/First", Order = 1)]
        public ActionResult Index()
        {
            return Content("");
        }    
    }

    public partial class RoutePrecedenceAmongActionsController : Controller
    {
        [GET("Route1", Precedence = 1)]
        public ActionResult Route1()
        {
            return Content("");
        }

        [GET("Route3")]
        public ActionResult Route3()
        {
            return Content("");
        }
    }

    public partial class RoutePrecedenceAmongActionsController
    {
        [GET("Route2", Precedence = 2)]
        public ActionResult Route2()
        {
            return Content("");
        }
    }

    public class RoutePrecedenceAmongControllers3Controller : Controller
    {
        [GET("Controller3/Index")]
        public ActionResult Index()
        {
            return Content("");
        }
    }

    public class RoutePrecedenceAmongControllers1Controller : Controller
    {
        [GET("Controller1/Index")]
        public ActionResult Index()
        {
            return Content("");
        }
    }

    public class RoutePrecedenceAmongControllers2Controller : Controller
    {
        [GET("Controller2/Index")]
        public ActionResult Index()
        {
            return Content("");
        }
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