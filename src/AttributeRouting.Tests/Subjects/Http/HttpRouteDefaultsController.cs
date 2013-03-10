using System.Web.Http;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Subjects.Http
{
    [RoutePrefix("HttpRouteDefaults")]
    public class HttpRouteDefaultsController : ApiController
    {
        [GET("Inline/{p=param}?{q=query}"), HttpGet]
        public string Inline(string p, string q)
        {
            return "HttpRouteDefaults.Inline({0}, {1})".FormatWith(p, q);
        }

        [GET("Optional/{p?}?{q?}"), HttpGet]
        public string Optional(string p = "", string q = "")
        {
            return "HttpRouteDefaults.Optional({0}, {1})".FormatWith(p, q);
        }

        [GET("{controller}/ControllerName", IsAbsoluteUrl = true), HttpGet]
        public string ControllerName()
        {
            return "HttpRouteDefaults.ControllerName";
        }

        [GET("{action}"), HttpGet]
        public string ActionName()
        {
            return "HttpRouteDefaults.ActionName";
        }
    }
}