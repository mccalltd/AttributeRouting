using System;
using System.Web.Http;
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

    [RouteArea("Cms", AreaUrl = "{culture}/Cms")]
    [RoutePrefix("Content")]
    public class Issue161TestController : Controller
    {
        [GET("Items?{p:int}")]
        public string Index(int p = 1)
        {
            return "";
        }
    }

    [RouteArea("Cms", AreaUrl = "{culture}/Cms")]
    [RoutePrefix("Content")]
    public class Issue161TestHttpController : ApiController
    {
        [GET("Items?{p:int}", RouteName = "Issue161TestHttp")]
        public string GetIndex(int p = 1)
        {
            return "";
        }
    }
}
