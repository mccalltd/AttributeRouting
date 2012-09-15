using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    public class HttpRoutePrecedenceAmongRoutesController : ApiController
    {
        [GET("Get/Second", Order = 2)]
        [GET("Get/Third", Order = 3)]
        [GET("Get/First", Order = 1)]
        [GET("Get/Fourth")]
        [GET("Get/Seventh", Order = -1)]
        [GET("Get/Fifth", Order = -3)]
        [GET("Get/Sixth", Order = -2)]
        public string Get()
        {
            return "";
        }
    }

    public partial class HttpRoutePrecedenceAmongActionsController
    {
        [GET("ApiRoute1", Precedence = 1)]
        public string Route1() { return ""; }

        [GET("ApiRoute3", Precedence = 3)]
        public string Route3() { return ""; }

        [GET("ApiRoute5", Precedence = -3)]
        public string Route5() { return ""; }

        [GET("ApiRoute7", Precedence = -1)]
        public string Route7() { return ""; }
    }

    public partial class HttpRoutePrecedenceAmongActionsController : ApiController
    {
        [GET("ApiRoute2", Precedence = 2)]
        public string Route2() { return ""; }

        [GET("ApiRoute4")]
        public string Route4() { return ""; }

        [GET("ApiRoute6", Precedence = -2)]
        public string Route6() { return ""; }
    }

    public class HttpRoutePrecedenceAmongControllers3Controller : ApiController
    {
        [GET("ApiController3/Get")]
        public string Get()
        {
            return "";
        }
    }

    public class HttpRoutePrecedenceAmongControllers1Controller : ApiController
    {
        [GET("ApiController1/Get")]
        public string Get()
        {
            return "";
        }
    }

    public class HttpRoutePrecedenceAmongControllers2Controller : ApiController
    {
        [GET("ApiController2/Get")]
        public string Get()
        {
            return "";
        }
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