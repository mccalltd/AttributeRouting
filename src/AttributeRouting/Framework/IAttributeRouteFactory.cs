using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting.Framework {
    public interface IAttributeRouteFactory {

        /// <summary>
        /// Create a new <see cref="IAttributeRoute"/>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="dataTokens"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IAttributeRoute CreateAttributeRoute<TConstraint, TController>(string url,
            IDictionary<string, object> defaults,
            IDictionary<string, TConstraint> constraints,
            IDictionary<string, object> dataTokens,
            AttributeRoutingConfiguration<TConstraint, TController> configuration);
    }
}
