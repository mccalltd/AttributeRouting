using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers
{
    public class RestfulController : ControllerBase
    {
        [GET("Resources")]
        [GET("Resources/Index")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("Resources/New")]
        public ActionResult New()
        {
            return View();
        }

        [PUT("Resources")]
        public ActionResult Create()
        {
            Flash("Resource Created");
            return RedirectToAction("Show", new { id = 1 });
        }

        [GET("Resources/{id}")]
        public ActionResult Show(int id)
        {
            return View();
        }

        [GET("Resources/{id}/Edit")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [POST("Resources/{id}")]
        public ActionResult Update(int id)
        {
            Flash("Resource Updated");
            return RedirectToAction("Show");
        }

        [GET("Resources/{id}/Delete")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        [DELETE("Resources/{id}")]
        public ActionResult Destroy(int id)
        {
            Flash("Resource Destroyed");
            return RedirectToAction("Index");
        }
    }
}
