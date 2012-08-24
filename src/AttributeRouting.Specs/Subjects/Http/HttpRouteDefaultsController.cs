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

        [GET("Optionals/{p1?}/{?p2}/{?p3?}")]
        public string Optionals(string p1, string p2, string p3)
        {
            return "";
        }
    }
}