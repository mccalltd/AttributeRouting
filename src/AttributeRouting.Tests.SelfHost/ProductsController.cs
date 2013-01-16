using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.SelfHost
{
    public class ProductsController : ApiController
    {
        [GET("Products")]
        public IEnumerable<Product> Get()
        {
            return new List<Product> 
            {
                new Product { Id = 1, Name = "Gizmo 1", Price = 1.99M },
                new Product { Id = 2, Name = "Gizmo 2", Price = 2.99M },
                new Product { Id = 3, Name = "Gizmo 3", Price = 3.99M }
            };
        }

        [GET("Product/{id}")]
        public Product Get(int id)
        {
            if (id < 1 || id > 3)
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }

            return new Product
            {
                Id = id,
                Name = "Gizmo " + id,
                Price = id + 0.99M
            };
        }
    }
}