using System.Web.Http;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Subjects.Http
{
    [RoutePrefix("HttpBasics")]
    public class HttpBasicsController : ApiController
    {
        [GET(""), HttpGet]
        public string Index()
        {
            return "HttpBasics.Index";
        }

        [POST(""), HttpPost]
        public string Create()
        {
            return "HttpBasics.Create";
        }

        [PUT("{id}"), HttpPut]
        public string Update(string id)
        {
            return "HttpBasics.Update({0})".FormatWith(id);
        }

        [DELETE("{id}"), HttpDelete]
        public string Delete(string id)
        {
            return "HttpBasics.Delete({0})".FormatWith(id);
        }
    }
}