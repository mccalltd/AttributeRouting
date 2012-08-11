using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RouteArea("ApiLocalization", AreaUrl = "api/{culture}/Localization")]
    [RoutePrefix("Prefix")]
    public class LocalizationController : ApiController
    {
        [GET("")]
        public string GetLocalized()
        {
            return "A Localized String";
        }
    }
}