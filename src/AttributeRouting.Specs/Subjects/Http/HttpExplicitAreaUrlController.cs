using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    [RouteArea("ApiArea", AreaUrl = "ApiExplicitArea")]
    public class HttpExplicitAreaUrlController : ApiController
    {
        [GET("Get")]
        public string Get()
        {
            return "";
        }

        [GET("ApiExplicitArea/DuplicatePrefix")]
        public string DuplicatePrefix()
        {
            return "";
        }
    }
}
