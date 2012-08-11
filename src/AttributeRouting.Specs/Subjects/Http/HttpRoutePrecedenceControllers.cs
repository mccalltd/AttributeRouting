using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    public class HttpRoutePrecedenceAmongRoutesController : ApiController
    {
        [GET("Get/Second", Order = 2)]
        [GET("Get/Third", Order = 3)]
        [GET("Get/First", Order = 1)]
        public string Get()
        {
            return "";
        }
    }

    public partial class HttpRoutePrecedenceAmongActionsController
    {
        [GET("ApiRoute2", Precedence = 2)]
        public string Route2()
        {
            return "";
        }
    }

    public partial class HttpRoutePrecedenceAmongActionsController : ApiController
    {
        [GET("ApiRoute1", Precedence = 1)]
        public string Route1()
        {
            return "";
        }

        [GET("ApiRoute3")]
        public string Route3()
        {
            return "";
        }
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