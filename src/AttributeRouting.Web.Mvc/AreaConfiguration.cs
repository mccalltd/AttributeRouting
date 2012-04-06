using System.Web.Mvc;

namespace AttributeRouting.Web.Mvc
{
    /// <summary>
    /// Helper for configuring areas when initializing AttributeRouting framework.
    /// </summary>
    public class AreaConfiguration : WebAreaConfiguration<IController, UrlParameter>
    {
        public AreaConfiguration(string name, AttributeRoutingConfiguration configuration) : base(name, configuration)
        {
        }
    }
}
