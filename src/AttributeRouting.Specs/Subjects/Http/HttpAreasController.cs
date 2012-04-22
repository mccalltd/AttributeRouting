using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    [RouteArea("ApiArea")]
    public class HttpAreasController : ApiController
    {
        [GET("Get")]
        public string Get()
        {
            return "";
        }

        [GET("ApiArea/DuplicatePrefix")]
        public string DuplicatePrefix()
        {
            return "";
        }

        [GET("ApiAreaAbsolute", IsAbsoluteUrl = true)]
        public string Absolute()
        {
            return "";
        }

        [GET("ApiAreas")]
        public string RouteBeginsWithAreaName()
        {
            return "";
        }
    }
}