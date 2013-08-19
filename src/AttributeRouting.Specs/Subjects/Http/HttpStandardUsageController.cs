using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    public class HttpStandardUsageController : ApiController
    {
        [GET("api")]
        public string Get()
        {
            return "";
        }

        [POST("api")]
        public string Post()
        {
            return "";
        }

        [PUT("api/{id}")]
        public string Put()
        {
            return "";
        }

        [DELETE("api/{id}")]
        public string Delete()
        {
            return "";
        }

        [PATCH("api/{id}")]
        public string Patch()
        {
            return "";
        }

        [GET("api/Wildcards/{*pathInfo}")]
        public string Wildcards()
        {
            return "";
        }

        [HttpRoute("api/AnyVerb")]
        public string AnyVerb()
        {
            return "";
        }
    }
}