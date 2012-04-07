using System.Web.Mvc;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Mvc.Framework.Factories;

namespace AttributeRouting.Web.Mvc
{
    public class AttributeRoutingConfiguration : WebAttributeRoutingConfiguration<IController, UrlParameter>
    {
        public AttributeRoutingConfiguration()
            : base(() => new MvcRouteHandler(), new MvcAttributeRouteFactory(), new ConstraintFactory(), new UrlParameterFactory())
        {           
        }
    }
}
