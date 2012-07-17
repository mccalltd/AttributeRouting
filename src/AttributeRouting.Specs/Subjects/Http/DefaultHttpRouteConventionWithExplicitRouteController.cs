using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    [DefaultHttpRouteConvention]
    public class DefaultHttpRouteConventionWithExplicitRouteController : ApiController
    {
        [GET("Legacy", IsAbsoluteUrl = true)]
        public string Get()
        {
            return "";
        }
    }
}