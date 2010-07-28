namespace AttributeRouting
{
    public class PUTAttribute : RouteAttribute
    {
        public PUTAttribute(string url) : base(url, "PUT") {}
    }
}