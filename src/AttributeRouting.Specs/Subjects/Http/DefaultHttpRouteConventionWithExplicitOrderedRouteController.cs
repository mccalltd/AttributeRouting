using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    [DefaultHttpRouteConvention]
    public class DefaultHttpRouteConventionWithExplicitOrderedRouteController : ApiController
    {
        [GET("Primary", Order = 1)]
        public string Get()
        {
            return "";
        }
    }
}