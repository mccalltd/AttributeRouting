using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    public class AttributeRoutingConfiguration : AspNetAttributeRoutingConfiguration<IController, UrlParameter>
    {
        public AttributeRoutingConfiguration()
            : base(() => new MvcRouteHandler())
        {           
        }
    }
}
