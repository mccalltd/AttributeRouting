using System.Web.Mvc;
using AttributeRouting.Mvc;

namespace AttributeRouting.Web.Controllers
{
    [RestfulRouteConvention]
    [RoutePrefix("Conventions")]
    public class RestfulConventionController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Create()
        {
            Flash("Resource Created");
            return RedirectToAction("Show", new { id = 1 });
        }

        public ActionResult Show(int id)
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Update(int id)
        {
            Flash("Resource Updated");
            return RedirectToAction("Show");
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        public ActionResult Destroy(int id)
        {
            Flash("Resource Destroyed");
            return RedirectToAction("Index");
        }
    }
}
