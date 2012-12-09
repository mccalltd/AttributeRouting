using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("Prefix")]
    public class RoutePrefixesController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("Prefix/DuplicatePrefix")]
        public ActionResult DuplicatePrefix()
        {
            return Content("");
        }

        [GET("PrefixAbsolute", IsAbsoluteUrl = true)]
        public ActionResult Absolute()
        {
            return Content("");
        }

        [GET("Prefixer")]
        public ActionResult RouteBeginsWithRoutePrefix()
        {
            return Content("");
        }

        [GET("NoPrefix", IgnoreRoutePrefix = true)]
        public string NoPrefix()
        {
            return "";
        }
    }

    [RouteArea("Area")]
    [RoutePrefix("Prefix")]
    public class AreaRoutePrefixesController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("");
        }

        [GET("Prefix/DuplicatePrefix")]
        public ActionResult DuplicatePrefix()
        {
            return Content("");
        }

        [GET("AreaPrefixAbsolute", IsAbsoluteUrl = true)]
        public ActionResult Absolute()
        {
            return Content("");
        }

        [GET("Area")]
        public string RelativeUrlIsAreaUrl()
        {
            return "";
        }
    }

    [RoutePrefix]
    public class DefaultRoutePrefixController : Controller
    {
        [GET("Index")]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}