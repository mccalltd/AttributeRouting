using System.Web.Mvc;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Mvc
{
    [RoutePrefix("BasicUsage")]
    public class BasicUsageController : Controller
    {
        [GET("")]
        public string Index()
        {
            return "BasicUsage.Index";
        }

        [POST("")]
        public string Create()
        {
            return "BasicUsage.Create";
        }

        [PUT("{id}")]
        public string Update(string id)
        {
            return "BasicUsage.Update({0})".FormatWith(id);
        }

        [DELETE("{id}")]
        public string Delete(string id)
        {
            return "BasicUsage.Delete({0})".FormatWith(id);
        }
    }
}
