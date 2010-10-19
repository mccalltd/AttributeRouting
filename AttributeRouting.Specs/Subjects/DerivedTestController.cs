using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public abstract class DerivedTestController : TestBaseController
    {
        public ActionResult Index()
        {
            return Content("");
        }
    }
}