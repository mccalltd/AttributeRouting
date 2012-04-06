namespace AttributeRouting.Http
{
    public class PUTAttribute : HttpRouteAttribute {
        public PUTAttribute(string routeUrl) 
            : base(routeUrl, "PUT") { }
    }
}
