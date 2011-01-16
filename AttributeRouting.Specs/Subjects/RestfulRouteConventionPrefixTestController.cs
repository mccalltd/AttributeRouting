using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RestfulRouteConvention]
    [RoutePrefix("Prefix")]
    public class RestfulRouteConventionPrefixTestController : Controller
    {
        public ActionResult Index()
        {
            return Content("");
        }

        public ActionResult New()
        {
            return Content("");
        }

        public ActionResult Create()
        {
            return Content("");
        }

        public ActionResult Show(int id)
        {
            return Content("");
        }

        public ActionResult Edit(int id)
        {
            return Content("");
        }

        public ActionResult Update(int id)
        {
            return Content("");
        }

        public ActionResult Delete(int id)
        {
            return Content("");
        }

        public ActionResult Destroy(int id)
        {
            return Content("");
        }
    }
}