using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RouteVersioned(MinVer = "1.0")]
    public class VersionedController : BaseApiController
    {
        //
        // GET: /Versioned/
        [GET("Versioned", MinVer = "0.0")]
        public string Index()
        {
            return "This is /versioned";
        }

        [GET("Versioned/{id}", MinVer="1.1")]
        public string Show(int id)
        {
            return string.Format("This is /versioned/id with id = {0}", id);
        }

        [GET("Versioned/SingleVersion", MinVer="1.0", MaxVer="1.0")]
        public string New()
        {
            return "This should only work with version 1.0";
        }

    }
}
