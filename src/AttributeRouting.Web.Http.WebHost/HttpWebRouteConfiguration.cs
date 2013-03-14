using AttributeRouting.Framework;

namespace AttributeRouting.Web.Http.WebHost
{
    public class HttpWebRouteConfiguration : HttpRouteConfigurationBase
    {
        public HttpWebRouteConfiguration(bool inMemory = false)
        {
            if (inMemory)
            {
                // Must turn on AutoGenerateRouteNames and use the Unique RouteNameBuilder for this to work out-of-the-box.
                AutoGenerateRouteNames = true;
                RouteNameBuilder = RouteNameBuilders.Unique;
            }
        }
    }
}
