using System;

namespace AttributeRouting
{
    /// <remarks>
    /// Implementors MUST disambiguate among routes based on the HttpMethods allowed for this route.
    /// In System.Web.Mvc, that means deriving from ActionMethodSelectorAttribute.
    /// In System.Web.Http, that means deriving from IActionHttpMethodProvider.
    /// </remarks>
    public interface IRouteAttribute 
    {
        /// <summary>
        /// The url for this action.
        /// </summary>
        string RouteUrl { get; }

        /// <summary>
        /// The HttpMethods this route is constrained against.
        /// </summary>
        string[] HttpMethods { get; }

        /// <summary>
        /// The order of this route among all the routes defined against this action.
        /// </summary>
        /// <remarks>
        /// Zero and Positive integers denote top routes: 1 is first, 2 is second, etc....
        /// Negative integers denote bottom routes: -1 is last, -2 is second to last, etc....
        /// </remarks>
        [Obsolete("Prefer ActionPrecedence for clarity of intent.")]
        int Order { get; set; }

        /// <summary>
        /// The order of this route among all the routes defined against this action.
        /// </summary>
        /// <remarks>
        /// Positive integers (including zero) denote top routes: 1 is first, 2 is second, etc....
        /// Negative integers denote bottom routes: -1 is last, -2 is second to last, etc....
        /// </remarks>
        int ActionPrecedence { get; set; }

        /// <summary>
        /// The order of this route among all the routes defined against this controller.
        /// </summary>
        /// <remarks>
        /// Positive integers (including zero) denote top routes: 1 is first, 2 is second, etc....
        /// Negative integers denote bottom routes: -1 is last, -2 is second to last, etc....
        /// </remarks>
        [Obsolete("Prefer ControllerPrecedence for clarity of intent.")]
        int Precedence { get; set; }

        /// <summary>
        /// The order of this route among all the routes defined against this controller.
        /// </summary>
        int ControllerPrecedence { get; set; }

        /// <summary>
        /// The order of this route among all the routes in the site.
        /// </summary>
        /// <remarks>
        /// Positive integers (including zero) denote top routes: 1 is first, 2 is second, etc....
        /// Negative integers denote bottom routes: -1 is last, -2 is second to last, etc....
        /// </remarks>
        int SitePrecedence { get; set; }

        /// <summary>
        /// The name this route will be registered with in the RouteTable.
        /// </summary>
        string RouteName { get; set; }

        /// <summary>
        /// If true, the generated route url will be applied from the root, skipping any relevant area name or route prefix.
        /// </summary>
        bool IsAbsoluteUrl { get; set; }

        /// <summary>
        /// Key used by translation provider to lookup the translation for the <see cref="RouteUrl"/>.
        /// </summary>
        string TranslationKey { get; set; }

        /// <summary>
        /// If set, will override <see cref="ConfigurationBase.UseLowercaseRoutes"/>
        /// set via global configuration for this route.
        /// </summary>
        bool UseLowercaseRoute { get; set; }

        /// <summary>
        /// Gets the tri-state value for UseLowercaseRoutes.
        /// </summary>
        bool? UseLowercaseRouteFlag { get; }

        /// <summary>
        /// If set, will override <see cref="ConfigurationBase.PreserveCaseForUrlParameters"/>
        /// set via global configuration for this route.
        /// </summary>
        bool PreserveCaseForUrlParameters { get; set; }

        /// <summary>
        /// Gets the tri-state value for PreserveCaseForUrlParameters.
        /// </summary>
        bool? PreserveCaseForUrlParametersFlag { get; }

        /// <summary>
        /// If true, will override <see cref="ConfigurationBase.AppendTrailingSlash"/>
        /// set via global configuration for this route.
        /// </summary>
        bool AppendTrailingSlash { get; set; }

        /// <summary>
        /// Gets the tri-state value for AppendTrailingSlash.
        /// </summary>
        bool? AppendTrailingSlashFlag { get; }

        /// <summary>
        /// If true, will ignore any route prefix specified via the <see cref="RoutePrefixAttribute"/>
        /// when building up the route URL.
        /// </summary>
        bool IgnoreRoutePrefix { get; set; }

        /// <summary>
        /// If true, will ignore any area URL prefix specified via the <see cref="RouteAreaAttribute"/>
        /// when building up the route URL.
        /// </summary>
        bool IgnoreAreaUrl { get; set; }


        /// <summary>
        /// Indicates if this route is versioned. Is set by defining the [RouteVersioned] attribute on the class
        /// </summary>
        bool IsVersioned { get; set; }

        /// <summary>
        /// The minimum version required. 
        /// </summary>
        SemanticVersion MinVersion { get; set; }

        /// <summary>
        /// The maximum version required.
        /// </summary>
        SemanticVersion MaxVersion { get; set; }
    }
}