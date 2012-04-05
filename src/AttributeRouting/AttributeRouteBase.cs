using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AttributeRouting.Framework;

namespace AttributeRouting {
    public abstract class AttributeRouteBase<TRoute> : IAttributeRoute {

        /// <summary>
        /// Underlying system route this wraps
        /// </summary>
        public abstract TRoute Route { get; }

        /// <summary>
        /// The route that a translated applies to.
        /// </summary>
        public AttributeRouteBase<TRoute> DefaultRoute { get; set; }

        /// <summary>
        /// The name of this route, for supporting named routes.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// The translations available for this route.
        /// </summary>
        public IEnumerable<AttributeRouteBase<TRoute>> Translations { get; set; }

        /// <summary>
        /// The culture name associated with this route.
        /// </summary>
        public string CultureName { get; set; }

        /// <summary>
        /// List of all the subdomains mapped via AttributeRouting.
        /// </summary>
        public List<string> MappedSubdomains { get; set; }

        /// <summary>
        /// The subdomain this route is to be applied against.
        /// </summary>
        public string Subdomain { get; set; }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        public abstract IDictionary<string, object> DataTokens { get; set; }    

    }
}
