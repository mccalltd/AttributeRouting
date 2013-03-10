using System.Web.Mvc;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Mvc
{
    [RoutePrefix("Basics")]
    public class BasicsController : Controller
    {
        [GET("")]
        public string Index()
        {
            return "Basics.Index";
        }

        [POST("")]
        public string Create()
        {
            return "Basics.Create";
        }

        [PUT("{id}")]
        public string Update(string id)
        {
            return "Basics.Update({0})".FormatWith(id);
        }

        [DELETE("{id}")]
        public string Delete(string id)
        {
            return "Basics.Delete({0})".FormatWith(id);
        }
    }
}
