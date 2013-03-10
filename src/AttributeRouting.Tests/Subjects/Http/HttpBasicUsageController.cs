using System.Web.Http;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Subjects.Http
{
    [RoutePrefix("HttpBasicUsage")]
    public class HttpBasicUsageController : ApiController
    {
        [GET(""), HttpGet]
        public string Index()
        {
            return "HttpBasicUsage.Index";
        }

        [POST(""), HttpPost]
        public string Create()
        {
            return "HttpBasicUsage.Create";
        }

        [PUT("{id}"), HttpPut]
        public string Update(string id)
        {
            return "HttpBasicUsage.Update({0})".FormatWith(id);
        }

        [DELETE("{id}"), HttpDelete]
        public string Delete(string id)
        {
            return "HttpBasicUsage.Delete({0})".FormatWith(id);
        }
    }
}