using System.Collections.Generic;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Abstraction used by <see cref="RouteBuilder"/> 
    /// to generate routes with custom functionality.
    /// </summary>
    /// <remarks>
    /// Due to different route implementations in
    /// System.Web.Routing (used for MVC controller routes) and
    /// System.Web.Http.Routing (used for Web API controllers).
    /// </remarks>
    public interface IAttributeRoute
    {
        /// <summary>
        /// If true, will override <see cref="ConfigurationBase.AppendTrailingSlash"/>
        /// set via global configuration and the generated route will have a trailing slash on the path of outbound URLs.
        /// </summary>
        bool? AppendTrailingSlash { get; set; }

        /// <summary>
        /// Constraints dictionary
        /// </summary>
        IDictionary<string, object> Constraints { get; set; }

        /// <summary>
        /// The culture name associated with this route.
        /// </summary>
        string CultureName { get; set; }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        IDictionary<string, object> DataTokens { get; set; }

        /// <summary>
        /// Defaults dictionary
        /// </summary>
        IDictionary<string, object> Defaults { get; set; }

        /// <summary>
        /// If true, will override <see cref="ConfigurationBase.PreserveCaseForUrlParameters"/>
        /// set via global configuration and the generated route will not lowercase URL parameter values.
        /// </summary>
        bool? PreserveCaseForUrlParameters { get; set; }

        /// <summary>
        /// Constraints dictionary for querystring constraints.
        /// </summary>
        IDictionary<string, object> QueryStringConstraints { get; set; }

        /// <summary>
        /// The name of this route, for supporting named routes.
        /// </summary>
        string RouteName { get; set; }

        /// <summary>
        /// The source-language route if this route is a translated route.
        /// </summary>
        IAttributeRoute SourceLanguageRoute { get; set; }

        /// <summary>
        /// The subdomain this route is to be applied against.
        /// </summary>
        string Subdomain { get; set; }

        /// <summary>
        /// The translations available for this route.
        /// </summary>
        IEnumerable<IAttributeRoute> Translations { get; set; }

        /// <summary>
        /// If true, will override <see cref="ConfigurationBase.UseLowercaseRoutes"/>
        /// set via global configuration and the generated route will have a lowercase URL.
        /// </summary>
        bool? UseLowercaseRoute { get; set; }

        /// <summary>
        /// Route URL
        /// </summary>
        string Url { get; set; }
    }
}