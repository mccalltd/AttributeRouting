using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    public abstract class InheritActionsApiControllerBase : BaseApiController
    {
        [GET("Base-Method"), HttpGet]
        public string Index()
        {
            return "Base Index";
        }
    }

    [RoutePrefix("Inherit/Derived")]
    public class InheritActionsDerivedApiController : InheritActionsApiControllerBase {}
}