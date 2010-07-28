using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected void Flash(string message)
        {
            TempData["flash"] = message;
        }
    }
}