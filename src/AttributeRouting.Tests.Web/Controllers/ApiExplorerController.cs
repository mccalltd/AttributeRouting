using System.Web.Mvc;
using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Controllers
{
    [RoutePrefix("Api/Explorer")]
    public class ApiExplorerController : Controller
    {
        [GET("")]
        [System.Web.Mvc.ActionName("Index")]
        public ActionResult Index101()
        {
            var explorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            return View(explorer);
        }
    }
}
