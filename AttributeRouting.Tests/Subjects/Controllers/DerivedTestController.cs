using System.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Controllers
{
    public abstract class DerivedTestController : TestBaseController
    {
        public ActionResult Index()
        {
            return Content("");
        }
    }
}