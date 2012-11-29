using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    public class QueryStringConstraintController : Controller
    {
        [GET("Books?author={author:int}")]
        public string GetBooksByAuthor(string author)
        {
            return "author: " + author;
        }

        [GET("Books?isbn={isbn:alpha}")]
        public string GetBooksByISBN(string isbn)
        {
            return "isbn: " + isbn;
        }
    }
}
