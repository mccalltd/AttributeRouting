namespace AttributeRouting
{
    public class DELETEAttribute : RouteAttribute
    {
        public DELETEAttribute(string url) : base(url, "DELETE") {}
    }
}