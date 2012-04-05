using System.Collections.Generic;

namespace AttributeRouting.Framework.Factories {
    public interface IAttributeRouteFactory<TConstraint, TController, TRoute, TRouteParameter, TRequestContext, TRouteData>
    {

        /// <summary>
        /// Create a new attribute route that wraps an underlying framework route
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        AttributeRouteContainerBase<TRoute> CreateAttributeRoute(string url,
            IDictionary<string, object> defaults,
            IDictionary<string, object> constraints,
            IDictionary<string, object> dataTokens,
            AttributeRoutingConfiguration<TConstraint, TController, TRoute, TRouteParameter, TRequestContext, TRouteData> configuration);
    }
}
