using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    public class HttpRoutePrecedenceAmongRoutesController : ApiController
    {
        [GET("Get/Second", ActionPrecedence = 2)]
        [GET("Get/Third", ActionPrecedence = 3)]
        [GET("Get/First", ActionPrecedence = 1)]
        [GET("Get/Fourth")]
        [GET("Get/Seventh", ActionPrecedence = -1)]
        [GET("Get/Fifth", ActionPrecedence = -3)]
        [GET("Get/Sixth", ActionPrecedence = -2)]
        public string Get()
        {
            return "";
        }
    }

    public partial class HttpRoutePrecedenceAmongActionsController
    {
        [GET("ApiRoute1", ControllerPrecedence = 1)]
        public string Route1() { return ""; }

        [GET("ApiRoute3", ControllerPrecedence = 3)]
        public string Route3() { return ""; }

        [GET("ApiRoute5", ControllerPrecedence = -3)]
        public string Route5() { return ""; }

        [GET("ApiRoute7", ControllerPrecedence = -1)]
        public string Route7() { return ""; }
    }

    public partial class HttpRoutePrecedenceAmongActionsController : ApiController
    {
        [GET("ApiRoute2", ControllerPrecedence = 2)]
        public string Route2() { return ""; }

        [GET("ApiRoute4")]
        public string Route4() { return ""; }

        [GET("ApiRoute6", ControllerPrecedence = -2)]
        public string Route6() { return ""; }
    }

    public class HttpRoutePrecedenceAmongControllers3Controller : ApiController
    {
        [GET("ApiController3/Get")]
        public string Get() { return ""; }
    }

    public class HttpRoutePrecedenceAmongControllers1Controller : ApiController
    {
        [GET("ApiController1/Get")]
        public string Get() { return ""; }
    }

    public class HttpRoutePrecedenceAmongControllers2Controller : ApiController
    {
        [GET("ApiController2/Get")]
        public string Get() { return ""; }
    }

    public class HttpRoutePrecedenceAmongControllers5Controller : ApiController
    {
        [GET("ApiController5/Get")]
        public string Get() { return ""; }
    }

    public class HttpRoutePrecedenceAmongControllers4Controller : ApiController
    {
        [GET("ApiController4/Get")]
        public string Get() { return ""; }
    }

    public class HttpRoutePrecedenceAmongTheSitesRoutesController : ApiController
    {
        [GET("The-First-Route", SitePrecedence = 1)]
        public string TheFirstRoute() { return "yay!"; }

        [GET("The-Last-Route", SitePrecedence = -1)]
        public string TheLastRoute() { return "nay!"; }
    }

    public class HttpRoutePrecedenceViaRoutePropertiesController : ApiController
    {
        [GET("ApiRoute3", ActionPrecedence = 1)]
        [GET("ApiRoute5", ActionPrecedence = -1)]
        [GET("ApiRoute4")]
        public string RouteWhatever() { return ""; }

        [GET("ApiRoute1", SitePrecedence = 1)]
        public string Route1() { return ""; }

        [GET("ApiRoute7", SitePrecedence = -1)]
        public string Route7() { return ""; }

        [GET("ApiRoute2", ControllerPrecedence = 1)]
        public string Route2() { return ""; }

        [GET("ApiRoute6", ControllerPrecedence = -1)]
        public string Route6() { return ""; }
    }

    public abstract class HttpRoutePrecedenceAmongDerivedControllersBaseController : ApiController { }

    public class HttpRoutePrecedenceAmongDerivedControllers1 : HttpRoutePrecedenceAmongDerivedControllersBaseController
    {
        [GET("ApiDerivedController1/Get")]
        public string Get()
        {
            return "";
        }
    }

    public class HttpRoutePrecedenceAmongDerivedControllers2 : HttpRoutePrecedenceAmongDerivedControllersBaseController
    {
        [GET("ApiDerivedController2/Get")]
        public string Get()
        {
            return "";
        }
    }
}