using System.Collections.Generic;

namespace AttributeRouting.Framework.Factories
{
    /// <summary>
    /// For frameworks to implement, a way to create a framework-specific attribute route
    /// </summary>
    public interface IAttributeRouteFactory
    {
        /// <summary>
        /// Create a new attribute route that wraps an underlying framework route
        /// </summary>
        IAttributeRoute CreateAttributeRoute(string url,
                                             IDictionary<string, object> defaults,
                                             IDictionary<string, object> constraints,
                                             IDictionary<string, object> dataTokens);
    }
}
