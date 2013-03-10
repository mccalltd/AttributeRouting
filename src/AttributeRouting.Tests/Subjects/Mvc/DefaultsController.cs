using System.Web.Mvc;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Mvc
{
    [RoutePrefix("Defaults")]
    public class DefaultsController : Controller
    {
        [GET("Inline/{p=param}?{q=query}")]
        public string Inline(string p, string q)
        {
            return "Defaults.Inline({0}, {1})".FormatWith(p, q);
        }

        [GET("Optional/{p?}?{q?}")]
        public string Optional(string p, string q)
        {
            return "Defaults.Optional({0}, {1})".FormatWith(p, q);
        }

        [GET("{controller}/ControllerName", IsAbsoluteUrl = true)]
        public string ControllerName()
        {
            return "Defaults.ControllerName";
        }

        [GET("{action}")]
        public string ActionName()
        {
            return "Defaults.ActionName";
        }
    }
}
