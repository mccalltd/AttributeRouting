using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting {
    public interface IRouteAttribute {

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
    }
}
