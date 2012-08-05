using System.Collections.Generic;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Abstraction used by <see cref="RouteBuilder"/> 
    /// to generate routes with custom functionality.
    /// </summary>
    /// <remarks>
    /// Due to different route implementations in
    /// System.Web.Routing (used for mvc controller routes),
    /// System.Web.Http.Routing (used for self-hosted api controllers), and 
    /// System.Web.Http.WebHost.Routing (used for web-hosted api controllers).
    /// </remarks>
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
        /// set via global configuration and the generated route will have a lowercase URL.
        /// </summary>
        bool? UseLowercaseRoute { get; set; }

        /// <summary>
        /// If true, will override <see cref="AttributeRoutingConfigurationBase.PreserveCaseForUrlParameters"/>
        /// set via global configuration and the generated route will not lowercase URL parameter values.
        /// </summary>
        bool? PreserveCaseForUrlParameters { get; set; }

        /// <summary>
        /// If true, will override <see cref="AttributeRoutingConfigurationBase.AppendTrailingSlash"/>
        /// set via global configuration and the generated route will have a trailing slash on the path of outbound URLs.
        /// </summary>
        bool? AppendTrailingSlash { get; set; }

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
        /// Default route container back-reference, used to organize route translations.
        /// </summary>
        IAttributeRoute DefaultRouteContainer { get; set; }
    }
}