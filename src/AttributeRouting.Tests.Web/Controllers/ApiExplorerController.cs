using System.Web.Http;
using System.Web.Mvc;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Controllers
{
    [RoutePrefix("Api/Explorer")]
    public class ApiExplorerController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            var explorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            return View(explorer);
        }
    }
}
