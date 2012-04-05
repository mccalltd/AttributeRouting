using System.Collections.Generic;

namespace AttributeRouting.Framework {
    public interface IAttributeRoute {

        /// <summary>
        /// Default route
        /// </summary>
        IAttributeRoute DefaultRoute { get; set; }

        /// <summary>
        /// The name of this route, for supporting named routes.
        /// </summary>
        string RouteName { get; set; }

        /// <summary>
        /// The translations available for this route.
        /// </summary>
        IEnumerable<IAttributeRoute> Translations { get; set; }

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

        string Url { get; set; }

        IDictionary<string, object> DataTokens { get; } 
    }
}