using AttributeRouting.Framework;

namespace AttributeRouting.Web.Http.SelfHost
{
    public class HttpSelfHostRouteConfiguration : HttpRouteConfigurationBase
    {
        public HttpSelfHostRouteConfiguration(bool inMemory = false)
        {
            // Must turn on AutoGenerateRouteNames and use the Unique RouteNameBuilder for this to work out-of-the-box.
            AutoGenerateRouteNames = true;
            RouteNameBuilder = RouteNameBuilders.Unique;
        }
    }
}
