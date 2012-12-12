using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    public class HttpRouteDefaultsController : ApiController
    {
        [GET("InlineDefaults/{hello=sun}/{goodnight=moon}")]
        public string InlineDefaults()
        {
            return "";
        }

        [GET("Optionals/{p1?}")]
        public string Optionals(string p1)
        {
            return "";
        }

        [GET("{controller}/{action}")]
        public string TheActionName()
        {
            return "is jack";
        }
    }
}