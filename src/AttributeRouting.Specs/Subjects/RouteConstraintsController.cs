using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RouteConstraintsController : Controller
    {
        [GET(@"InlineConstraints/{number(\d+)}/{word(\w{2})}/{alphanum([A-Za-z0-9]*)}/{capture((gotcha))}")]
        public ActionResult InlineConstraints(long number, string word)
        {
            return Content("");
        }
    }
}