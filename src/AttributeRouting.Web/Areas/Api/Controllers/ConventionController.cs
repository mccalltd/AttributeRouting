using System.Collections.Generic;
using AttributeRouting.AspNet.Constraints;
using AttributeRouting.Http;

namespace AttributeRouting.Web.Areas.Api.Controllers
{
    [HttpRouteConvention]
    public class ConventionController : BaseApiController
    {
        // GET /api/<controller>
        public IEnumerable<string> GetConventions()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/<controller>/5
        public string GetConvention(int id)
        {
            return "GET {id} = " + id;
        }

        // POST /api/<controller>        
        public string PostConvention(string value)
        {
            return "POST {value} = " + value;
        }

        // PUT /api/<controller>/5
        public string PutConvention(int id, string value)
        {
            return "PUT {id} = " + id + ", {value} = " + value;
        }

        // DELETE /api/<controller>/5
        public string DeleteConvention(int id)
        {
            return "DELETE {id} = " + id;
        }
    }
}