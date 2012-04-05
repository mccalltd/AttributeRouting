using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting.WebApi;

namespace AttributeRouting.Web.Areas.Api.Controllers
{
    public class PlainController : ApiController
    {
        // GET /api/plain
        [GET("/api/plain")]
        public IEnumerable<string> Get()
        {
            return new [] { "value1", "value2" };
        }

        // GET /api/plain/5
        [GET("/api/plain/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/plain
        [POST("/api/plain")]
        public void Post(string value)
        {
        }

        // PUT /api/plain/5
        [PUT("/api/plain/{id}")]
        public void Put(int id, string value)
        {
        }

        // DELETE /api/plain/5
        [DELETE("/api/plain/{id}")]
        public void Delete(int id)
        {
        }
    }
}