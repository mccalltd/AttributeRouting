using System.Web.Mvc;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Mvc
{
    [RoutePrefix("RouteDefaults")]
    public class RouteDefaultsController : Controller
    {
        [GET("Inline/{p=param}?{q=query}")]
        public string Inline(string p, string q)
        {
            return "RouteDefaults.Inline({0}, {1})".FormatWith(p, q);
        }

        [GET("Optional/{p?}?{q?}")]
        public string Optional(string p, string q)
        {
            return "RouteDefaults.Optional({0}, {1})".FormatWith(p, q);
        }

        [GET("{controller}/ControllerName", IsAbsoluteUrl = true)]
        public string ControllerName()
        {
            return "RouteDefaults.ControllerName";
        }

        [GET("{action}")]
        public string ActionName()
        {
            return "RouteDefaults.ActionName";
        }
    }
}
