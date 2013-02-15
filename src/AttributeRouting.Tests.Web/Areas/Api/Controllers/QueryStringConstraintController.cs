using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    public class QueryStringConstraintApiController : ApiController
    {
        [GET("api/Books?{author}")]
        public string GetBooksByAuthor(string author)
        {
            return "author: " + author;
        }

        [GET("api/Books?{isbn}&{x:int}")]
        public string GetBooksByISBN(string isbn)
        {
            return "isbn: " + isbn;
        }
    }
}
