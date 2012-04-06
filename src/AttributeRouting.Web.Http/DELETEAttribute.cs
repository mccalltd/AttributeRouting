namespace AttributeRouting.Web.Http {
    public class DELETEAttribute : HttpRouteAttribute {
        public DELETEAttribute(string routeUrl)
            : base(routeUrl, "DELETE")
        {
            
        }
    }
}
