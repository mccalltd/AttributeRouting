using System.Collections.Generic;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RoutePrefix("plain")]
    public class PlainApiController : BaseApiController
    {
        // GET /api/plain
        [GET("")]
        public IEnumerable<string> All()
        {
            return new [] { "value1", "value2" };
        }

        // GET /api/plain/5
        [GET("{id}")]
        public string Single(int id)
        {
            return id.ToString();
        }

        // POST /api/plain
        [POST("")]
        public void Create()
        {
        }

        // PUT /api/plain/5
        [PUT("{id}")]
        public void Update(int id)
        {
        }

        // DELETE /api/plain/5
        [DELETE("{id}")]
        public void Destroy(int id)
        {
        }
    }
}