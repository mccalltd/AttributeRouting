namespace AttributeRouting
{
    /// <remarks>
    /// Implementors MUST disambiguate among routes based on the HttpMethods allowed for this route.
    /// In System.Web.Mvc, that means deriving from ActionMethodSelectorAttribute.
    /// In System.Web.Http, that means deriving from IActionMethodSelectorAttribute.
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
        int Order { get; set; }

        /// <summary>
        /// The order of this route among all the routes defined against this controller.
        /// </summary>
        int Precedence { get; set; }

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
        /// If set, will override <see cref="AttributeRoutingConfigurationBase.UseLowercaseRoutes"/>
        /// set via global configuration for this route.
        /// </summary>
        bool UseLowercaseRoute { set; }

        /// <summary>
        /// Gets the tri-state value for UseLowercaseRoutes.
        /// </summary>
        bool? UseLowercaseRouteFlag { get; }

        /// <summary>
        /// If set, will override <see cref="AttributeRoutingConfigurationBase.PreserveCaseForUrlParameters"/>
        /// set via global configuration for this route.
        /// </summary>
        bool PreserveCaseForUrlParameters { set; }

        /// <summary>
        /// Gets the tri-state value for PreserveCaseForUrlParameters.
        /// </summary>
        bool? PreserveCaseForUrlParametersFlag { get; }

        /// <summary>
        /// If true, will override <see cref="AttributeRoutingConfigurationBase.AppendTrailingSlash"/>
        /// set via global configuration for this route.
        /// </summary>
        bool AppendTrailingSlash { set; }

        /// <summary>
        /// Gets the tri-state value for AppendTrailingSlash.
        /// </summary>
        bool? AppendTrailingSlashFlag { get; }
    }
}