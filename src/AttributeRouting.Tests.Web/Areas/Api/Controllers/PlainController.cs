using System.Collections.Generic;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RoutePrefix("plain")]
    public class PlainController : BaseApiController
    {
        // GET /api/plain
        [GET("")]
        public IEnumerable<string> GetAll()
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
        public void Post()
        {
        }

        // PUT /api/plain/5
        [PUT("{id}")]
        public void Put(int id)
        {
        }

        // DELETE /api/plain/5
        [DELETE("{id}")]
        public void Delete(int id)
        {
        }

        // PATCH /api/plain/5
        [PATCH("{id}")]
        public void Patch()
        {
        }
    }
}