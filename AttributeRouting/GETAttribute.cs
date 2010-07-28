namespace AttributeRouting
{
    public class GETAttribute : RouteAttribute
    {
        public GETAttribute(string url) : base(url, "GET") {}
    }
}