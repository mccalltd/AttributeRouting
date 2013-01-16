using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    public class QueryStringConstraintController : Controller
    {
        [GET("Books?{author}")]
        public string GetBooksByAuthor(string author, string damage)
        {
            return "author: " + author;
        }

        [GET("Books?{isbn}&{x:int}")]
        public string GetBooksByISBN(string isbn)
        {
            return "isbn: " + isbn;
        }
    }
}
