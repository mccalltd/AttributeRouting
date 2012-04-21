using System.Collections.Generic;
using AttributeRouting.Web;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RoutePrefix("plain")]
    public class PlainController : BaseApiController
    {
        // GET /api/plain
        [GET("")]
        public IEnumerable<string> Get()
        {
            return new [] { "value1", "value2" };
        }

        // GET /api/plain/5
        [GET("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/plain
        [POST("")]
        [POST("alt1")]
        [RegexRouteConstraint("value", "somePattern")]
        public void Post(string value)
        {
        }

        // PUT /api/plain/5
        [PUT("{id}")]
        public void Put(int id, string value)
        {
        }

        // DELETE /api/plain/5
        [DELETE("{id}")]
        public void Delete(int id)
        {
        }
    }
}