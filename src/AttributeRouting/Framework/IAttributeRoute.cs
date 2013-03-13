using System.Collections.Generic;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Abstraction used by <see cref="RouteBuilder"/> to generate routes with custom functionality.
    /// </summary>
    /// <remarks>
    /// Due to different route implementations in
    /// System.Web.Routing (used for MVC controller routes) and
    /// System.Web.Http.Routing (used for Web API controllers).
    /// </remarks>
    public interface IAttributeRoute
    {
        /// <summary>
        /// Constraints dictionary
        /// </summary>
        IDictionary<string, object> Constraints { get; set; }

        /// <summary>
        /// DataTokens dictionary
        /// </summary>
        IDictionary<string, object> DataTokens { get; set; }

        /// <summary>
        /// Defaults dictionary
        /// </summary>
        IDictionary<string, object> Defaults { get; set; }

        /// <summary>
        /// Constraints dictionary for querystring constraints.
        /// </summary>
        IDictionary<string, object> QueryStringConstraints { get; set; }

        /// <summary>
        /// Defaults dictionary for querystring defaults.
        /// </summary>
        IDictionary<string, object> QueryStringDefaults { get; set; }

        /// <summary>
        /// The name of this route, for supporting named routes.
        /// </summary>
        string RouteName { get; set; }

        /// <summary>
        /// Route URL
        /// </summary>
        string Url { get; set; }
    }
}