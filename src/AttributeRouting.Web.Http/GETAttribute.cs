namespace AttributeRouting.Web.Http
{
    public class GETAttribute : HttpRouteAttribute {
        public GETAttribute(string routeUrl) 
            : base(routeUrl, "GET", "HEAD") { }
    }
}
