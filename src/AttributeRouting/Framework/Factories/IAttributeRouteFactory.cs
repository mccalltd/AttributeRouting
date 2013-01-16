using System.Collections.Generic;

namespace AttributeRouting.Framework.Factories
{
    /// <summary>
    /// Abstraction used by <see cref="RouteBuilder"/>
    /// to put custom routes in the underlying route table.
    /// </summary>
    /// <remarks>
    /// Due to different route implementations in
    /// System.Web.Routing (used for mvc controller routes),
    /// System.Web.Http.Routing (used for self-hosted api controllers), and 
    /// System.Web.Http.WebHost.Routing (used for web-hosted api controllers).
    /// </remarks>
    public interface IAttributeRouteFactory
    {
        /// <summary>
        /// Create attribute routes from the given metadata.
        /// </summary>
        IEnumerable<IAttributeRoute> CreateAttributeRoutes(string url, IDictionary<string, object> defaults, IDictionary<string, object> constraints, IDictionary<string, object> dataTokens);
    }
}
