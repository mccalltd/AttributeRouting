using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Mixins for use int implementations of IAttributeRoute.
    /// </summary>
    public static class AttributeRouteExtensions
    {
        /// <summary>
        /// Gets the translated virtual path for this route.
        /// </summary>
        /// <typeparam name="TVirtualPathData">
        /// The type of virtual path data to be returned. 
        /// This varies based on whether the route is a
        /// System.Web.Routing.Route or System.Web.Http.Routing.HttpRoute.
        /// </typeparam>
        /// <param name="route"></param>
        /// <param name="fromTranslation">A delegate that can get the TVirtualPathData from a translated route</param>
        /// <returns>Returns null if no translation is available.</returns>
        public static TVirtualPathData GetTranslatedVirtualPath<TVirtualPathData>(
            this IAttributeRoute route,
            Func<IAttributeRoute, TVirtualPathData> fromTranslation
            ) where TVirtualPathData: class
        {
            var translations = (route.Translations ?? Enumerable.Empty<IAttributeRoute>()).ToList();
            if (!translations.Any())
                return null;

            var currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            // Try and get the language-culture translation, then fall back to language translation
            var translation = translations.FirstOrDefault(t => t.CultureName == currentCultureName)
                              ?? translations.FirstOrDefault(t => currentCultureName.StartsWith(t.CultureName));

            if (translation == null)
                return null;

            return fromTranslation(translation);
        }

        /// <summary>
        /// Performs lowercasing and appends trailing slash if this route is so configured.
        /// </summary>
        /// <param name="route"></param>
        /// <param name="virtualPath">The current virtual path, after translation</param>
        /// <param name="configuration">The configuration for the route</param>
        /// <returns>The final virtual path</returns>
        public static string GetFinalVirtualPath(this IAttributeRoute route, string virtualPath, AttributeRoutingConfigurationBase configuration)
        {
            /**
             * Lowercase urls.
             * NOTE: The initial lowercasing of all BUT url params occurs in RouteBuilder.CreateRouteUrl().
             * This is just a final lowercasing of the final, parameter-replaced url.
             */

            var lower = route.UseLowercaseRoute.HasValue
                            ? route.UseLowercaseRoute.Value
                            : configuration.UseLowercaseRoutes;
            
            var preserve = route.PreserveCaseForUrlParameters.HasValue
                               ? route.PreserveCaseForUrlParameters.Value
                               : configuration.PreserveCaseForUrlParameters;
            
            if (lower && !preserve)
                virtualPath = TransformVirtualPathToLowercase(virtualPath);

            /**
             * Append trailing slashes
             */

            var appendTrailingSlash = route.AppendTrailingSlash.HasValue
                                          ? route.AppendTrailingSlash.Value
                                          : configuration.AppendTrailingSlash;

            if (appendTrailingSlash)
                virtualPath = AppendTrailingSlashToVirtualPath(virtualPath);

            return virtualPath;
        }

        private static string TransformVirtualPathToLowercase(string virtualPath)
        {
            string path, query;
            GetPathAndQuery(virtualPath, out path, out query);

            return path.ToLowerInvariant() + query;
        }

        private static string AppendTrailingSlashToVirtualPath(string virtualPath)
        {
            string path, query;
            GetPathAndQuery(virtualPath, out path, out query);

            if (path.HasValue() && !path.EndsWith("/"))
                path += "/";

            return path + query;
        }

        private static void GetPathAndQuery(string virtualPath, out string path, out string query)
        {
            // NOTE: Do not lowercase the querystring vals
            var match = Regex.Match(virtualPath, @"(?<path>[^\?]*)(?<query>\?.*)?");

            // Just covering my backside here in case the regex fails for some reason.
            if (!match.Success)
            {
                path = virtualPath;
                query = null;
            }
            else
            {
                path = match.Groups["path"].Value;
                query = match.Groups["query"].Value;
            }
        }
    }
}
