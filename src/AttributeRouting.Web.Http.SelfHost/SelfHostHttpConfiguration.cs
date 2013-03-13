using AttributeRouting.Framework;

namespace AttributeRouting.Web.Http.SelfHost
{
    public class SelfHostHttpConfiguration : HttpConfiguration
    {
        public SelfHostHttpConfiguration()
        {
            // AutoGenerateRouteNames config setting initialization
            AutoGenerateRouteNames = true;
            RouteNameBuilder = new UniqueRouteNameBuilder();
        }
    }
}
