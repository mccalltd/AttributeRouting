using System.Collections.Generic;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Abstraction used by <see cref="RouteBuilder"/> when generating an <see cref="IAttributeRoute"/>.
    /// </summary>
    /// <remarks>
    /// Due to different route implementations in
    /// System.Web.Routing (used for mvc controller routes) and
    /// System.Web.Http.Routing (used for web api controllers).
    /// </remarks>
    public interface IAttributeRouteFactory
    {
        /// <summary>
        /// Create attribute routes from the given metadata.
        /// </summary>
        IEnumerable<IAttributeRoute> CreateAttributeRoutes(string url, IDictionary<string, object> defaults, IDictionary<string, object> constraints, IDictionary<string, object> dataTokens);
    }
}
