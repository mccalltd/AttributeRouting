using System.Web.Http;
using System.Web.Http.Controllers;

namespace AttributeRouting.Web.Http.WebHost
{
    /// <summary>
    /// Helper for configuring areas when initializing AttributeRouting framework.
    /// </summary>
    public class HttpAreaConfiguration : WebAreaConfiguration<RouteParameter>
    {
        public HttpAreaConfiguration(string name, HttpAttributeRoutingConfiguration configuration) 
            : base(name, configuration)
        {
        }
    }
}
