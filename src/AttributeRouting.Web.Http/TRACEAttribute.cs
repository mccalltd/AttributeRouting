namespace AttributeRouting.Web.Http
{
    public class TRACEAttribute : HttpRouteAttribute {
        public TRACEAttribute(string routeUrl)
            : base(routeUrl, "TRACE")
        {
            
        }
    }
}
