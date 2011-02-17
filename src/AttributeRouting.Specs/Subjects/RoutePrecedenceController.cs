using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    public class RoutePrecedenceAmongRoutesController : Controller
    {
        [GET("Index/Second", Order = 2)]
        [GET("Index/Third", Order = 3)]
        [GET("Index/First", Order = 1)]
        public ActionResult Index()
        {
            return Content("");
        }    
    }

    public partial class RoutePrecedenceAmongActionsController : Controller
    {
        [GET("Route1", Precedence = 1)]
        public ActionResult Route1()
        {
            return Content("");
        }

        [GET("Route3")]
        public ActionResult Route3()
        {
            return Content("");
        }
    }

    public partial class RoutePrecedenceAmongActionsController
    {
        [GET("Route2", Precedence = 2)]
        public ActionResult Route2()
        {
            return Content("");
        }
    }
}