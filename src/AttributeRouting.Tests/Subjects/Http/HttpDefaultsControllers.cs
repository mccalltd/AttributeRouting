using System.Web.Http;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Subjects.Http
{
    [RoutePrefix("HttpDefaults")]
    public class HttpDefaultsController : ApiController
    {
        [GET("Inline/{p=param}?{q=query}"), HttpGet]
        public string Inline(string p, string q)
        {
            return "HttpDefaults.Inline({0}, {1})".FormatWith(p, q);
        }

        [GET("Optional/{p?}?{q?}"), HttpGet]
        public string Optional(string p = "", string q = "")
        {
            return "HttpDefaults.Optional({0}, {1})".FormatWith(p, q);
        }

        [GET("{controller}/ControllerName", IsAbsoluteUrl = true), HttpGet]
        public string ControllerName()
        {
            return "HttpDefaults.ControllerName";
        }

        [GET("{action}"), HttpGet]
        public string ActionName()
        {
            return "HttpDefaults.ActionName";
        }
    }

    [RouteArea("HttpDefaults", AreaUrl = "HttpDefaults/InAreaPrefix/{p=param}")]
    public class HttpDefaultsInAreaPrefixController : ApiController
    {
        [GET(""), HttpGet]
        public string Index(string p)
        {
            return "HttpDefaultsInAreaPrefix.Index({0})".FormatWith(p);
        }
    }

    [RoutePrefix("HttpDefaults/InRoutePrefix/{p=param}")]
    public class HttpDefaultsInRoutePrefixController : ApiController
    {
        [GET(""), HttpGet]
        public string Index(string p)
        {
            return "HttpDefaultsInRoutePrefix.Index({0})".FormatWith(p);
        }
    }
}