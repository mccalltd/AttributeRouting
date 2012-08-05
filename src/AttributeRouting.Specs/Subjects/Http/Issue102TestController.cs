using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    [RoutePrefix("Issue102")]
    public class Issue102TestController : ApiController
    {
        [GET("")]
        public string Get()
        {
            return "";
        }

        [GET("{id}")]
        public string Get(int id)
        {
            return "";
        }
    }
}