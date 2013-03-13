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
        /// If true, will ignore any route prefix specified via the <see cref="RoutePrefixAttribute"/>
        /// when building up the route URL.
        /// </summary>
        bool IgnoreRoutePrefix { get; set; }

        /// <summary>
        /// If true, will ignore any area URL prefix specified via the <see cref="RouteAreaAttribute"/>
        /// when building up the route URL.
        /// </summary>
        bool IgnoreAreaUrl { get; set; }
    }
}