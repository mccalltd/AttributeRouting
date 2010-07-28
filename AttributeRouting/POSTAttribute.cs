namespace AttributeRouting
{
    public class POSTAttribute : RouteAttribute
    {
        public POSTAttribute(string url) : base(url, "POST") {}
    }
}