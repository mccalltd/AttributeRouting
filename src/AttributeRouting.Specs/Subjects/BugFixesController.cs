using System;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("BugFixes")]
    public class BugFixesController : Controller
    {
        [GET("Gallery/_CenterImage/{guid_Gallery?}/{slideShow?}/{currentController?}/{image?}")]
        public ActionResult Issue43_OptionalParamsAreMucky(Guid? guid_Gallery, bool? slideShow, string currentController, string image)
        {
            return Content("I'm fixed!");
        }
    }
}
