using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    public class AttributeRoutingConfiguration : WebAttributeRoutingConfiguration<IController, UrlParameter>
    {
        public AttributeRoutingConfiguration()
            : base(() => new MvcRouteHandler())
        {           
        }
    }
}
