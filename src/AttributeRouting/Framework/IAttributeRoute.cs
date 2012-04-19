using System.Collections.Generic;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Generic interface for AttributeRoutes (logging, etc.) that doesn't require a Route type
    /// </summary>
    public interface IAttributeRoute
    {
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
        /// Route URL
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// If true, will override <see cref="AttributeRoutingConfigurationBase.UseLowercaseRoutes"/>
        /// set via global configuration
        /// and the generated route will have a lowercase URL.
        /// </summary>
        bool UseLowercaseRoute { get; set; }

        /// <summary>
        /// If true, will override <see cref="AttributeRoutingConfigurationBase.PreserveCaseForUrlParameters"/>
        /// set via global configuration
        /// and the generated route will not lowercase URL parameter values.
        /// </summary>
        bool PreserveCaseForUrlParameters { get; set; }

        /// <summary>
        /// If true, will override <see cref="AttributeRoutingConfigurationBase.AppendTrailingSlash"/>
        /// set via global configuration
        /// and the generated route will have a trailing slash on the path of outbound URLs.
        /// </summary>
        bool AppendTrailingSlash { get; set; }

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

        /// <summary>
        /// The translations available for this route.
        /// </summary>
        IEnumerable<IAttributeRoute> Translations { get; set; }

        /// <summary>
        /// Default route container back-reference
        /// </summary>
        IAttributeRoute DefaultRouteContainer { get; set; }
    }
}