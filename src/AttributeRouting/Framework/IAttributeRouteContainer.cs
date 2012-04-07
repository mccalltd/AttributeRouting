using System.Collections.Generic;

namespace AttributeRouting.Framework {

    /// <summary>
    /// Generic interface for AttributeRoutes (logging, etc.) that doesn't
    /// require a Route type
    /// </summary>
    public interface IAttributeRouteContainer {
        /// <summary>
        /// The name of this route, for supporting named routes.
        /// </summary>
        string RouteName { get; set; }

        /// <summary>
        /// The culture name associated with this route.
        /// </summary>
        string CultureName { get; set; }

        /// <summary>
        /// List of all the subdomains mapped via AttributeRouting.
        /// </summary>
        List<string> MappedSubdomains { get; set; }

        /// <summary>
        /// The subdomain this route is to be applied against.
        /// </summary>
        string Subdomain { get; set; }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        IDictionary<string, object> DataTokens { get; set; }

        /// <summary>
        /// Constraints dictionary
        /// </summary>
        IDictionary<string, object> Constraints { get; set; }

        /// <summary>
        /// Defaults dictionary
        /// </summary>
        IDictionary<string, object> Defaults { get; set; }
    }
}