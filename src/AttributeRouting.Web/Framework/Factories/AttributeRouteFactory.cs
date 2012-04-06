using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Framework.Factories {
    public abstract class AttributeRouteFactory<TController, TParameter> : IAttributeRouteFactory<IRouteConstraint, TController, AttributeRoute<TController, TParameter>, TParameter, HttpContextBase, RouteData>
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
        public abstract AttributeRouteContainerBase<AttributeRoute<TController, TParameter>>
            CreateAttributeRoute(
            string url,
            IDictionary<string, object> defaults,
            IDictionary<string, object> constraints,
            IDictionary<string, object> dataTokens,
            AttributeRoutingConfiguration
                <IRouteConstraint, TController, AttributeRoute<TController, TParameter>, TParameter,
                HttpContextBase, RouteData> configuration);
    }
}
