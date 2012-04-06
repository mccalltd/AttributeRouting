namespace AttributeRouting.Web.Http
{
    public class OPTIONSAttribute : HttpRouteAttribute {
        public OPTIONSAttribute(string routeUrl)
            : base(routeUrl, "OPTIONS")
        {
            
        }
    }
}
