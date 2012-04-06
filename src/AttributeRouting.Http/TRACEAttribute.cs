namespace AttributeRouting.Http
{
    public class TRACEAttribute : HttpRouteAttribute {
        public TRACEAttribute(string routeUrl)
            : base(routeUrl, "TRACE")
        {
            
        }
    }
}
