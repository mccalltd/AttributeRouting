using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Visitor-ish used to extend implementations of <see cref="IAttributeRoute"/>.
    /// </summary>
    /// <remarks>
    /// Due to different route implementations in
    /// System.Web.Routing (used for MVC controller routes) and
    /// System.Web.Http.Routing (used for Web API controller routes). 
    /// </remarks>
    public class AttributeRouteVisitor
    {
        private readonly IAttributeRoute _route;
        private readonly ConfigurationBase _configuration;

        /// <summary>
        /// Creates a new visitor extending implementations of IAttributeRoute with common logic.
        /// </summary>
        /// <param name="route">The route</param>
        /// <param name="configuration">The route's configuration</param>
        public AttributeRouteVisitor(IAttributeRoute route, ConfigurationBase configuration)
        {
            if (route == null) throw new ArgumentNullException("route");
            if (configuration == null) throw new ArgumentNullException("configuration");

            _route = route;
            _configuration = configuration;
        }

        /// <summary>
        /// Optimizes route matching by comparing the static left part of a route's URL with the requested path.
        /// </summary>
        /// <param name="requestedPath">The path of the requested URL.</param>
        /// <returns>True if the requested URL path starts with the static left part of the route's URL.</returns>
        /// <remarks>Thanks: http://samsaffron.com/archive/2011/10/13/optimising-asp-net-mvc3-routing </remarks>
        public bool IsLeftPartOfUrlMatched(string requestedPath)
        {
            var routePath = _route.Url;
            var indexOfFirstParam = routePath.IndexOf("{", StringComparison.OrdinalIgnoreCase);
            var leftPart = indexOfFirstParam == -1 ? routePath : routePath.Substring(0, indexOfFirstParam);
            var comparableLeftPart = leftPart.TrimEnd('/');

            // Compare the left part with the requested path
            var comparableRequestedPath = requestedPath.TrimEnd('/');
            return comparableRequestedPath.StartsWith(comparableLeftPart, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Tests whether the configured subdomain (if any) matches the current host.
        /// </summary>
        /// <param name="requestedSubdomain">The subdomain part of the host from the current request</param>
        /// <returns>True if the subdomain for this route matches the current request host.</returns>
        public bool IsSubdomainMatched(string requestedSubdomain)
        {
            // If no subdomains are mapped with AR, then yes.
            if (!_configuration.MappedSubdomains.Any())
            {
                return true;
            }

            // Match if subdomain is null and this route has no subdomain.
            if (requestedSubdomain.HasNoValue() && _route.Subdomain.HasNoValue())
            {
                return true;
            }

            // Match if this route is mapped to the requested host's subdomain
            var routeSubdomain = _route.Subdomain ?? _configuration.DefaultSubdomain;
            if (routeSubdomain.ValueEquals(requestedSubdomain))
            {
                return true;
            }

            // Otherwise, this route does not match the request.
            return false;
        }

        /// <summary>
        /// Tests whether the route matches the current UI culture.
        /// </summary>
        /// <param name="cultureName">The name of the UI culture to test to test.</param>
        /// <returns></returns>
        public bool IsCultureNameMatched(string cultureName)
        {
            // If not constraining by culture, then do not apply this check.
            if (!_configuration.ConstrainTranslatedRoutesByCurrentUICulture)
            {
                return true;
            }

            // If no translations are available, then true.
            if (!_configuration.TranslationProviders.Any())
            {
                return true;
            }

            // Need the neutral culture as a fallback during matching.
            var neutralCultureName = cultureName.Split('-').First();

            if (_route.SourceLanguageRoute == null)
            {
                // This is a source language route:
                // Match if this route has no translations.
                var translations = _route.Translations.ToArray();
                if (!translations.Any())
                {
                    return true;
                }

                // Match if this route has no translations for the current UI culture's language.
                if (!translations.Any(t => t.CultureName.ValueEquals(neutralCultureName)))
                {
                    return true;
                }
            }
            else
            {
                // This is a translated route:

                var routeCultureName = _route.CultureName;

                // Match if the current UI culture is the culture of this route.
                if (cultureName.ValueEquals(routeCultureName))
                {
                    return true;
                }

                // Match if:
                // - the route's culture name is neutral,
                // - and it matches the current UI culture's language,
                // - and no translation exists for the specific current UI culture.
                if (routeCultureName.Split('-').Length == 1 /* neutral culture name */
                    && neutralCultureName == routeCultureName /* matches the current UI culture's language */
                    && !_route.SourceLanguageRoute.Translations.Any(t => t.CultureName.ValueEquals(cultureName)))
                {
                    return true;
                }
            }

            // Otherwise, don't match.
            return false;
        }

        /// <summary>
        /// Wraps calls to the GetVirtualPath method of routes, removing querystring route constraints
        /// that would otherwise lead to generated routes with invalid urls.
        /// </summary>
        /// The type of virtual path data to be returned. 
        /// This varies based on whether the route is a
        /// System.Web.Routing.Route or System.Web.Http.Routing.HttpRoute.
        /// <param name="fromBaseMethod">The base GetVirtualPath method call</param>
        /// <returns>The result from the base call</returns>
        public TVirtualPathData GetVirtualPath<TVirtualPathData>(Func<TVirtualPathData> fromBaseMethod)
            where TVirtualPathData : class
        {
            // Remove querystring route constraints:
            // the base GetVirtualPath will not inject route params that have constraints into the querystring.
            var queryStringConstraints = new Dictionary<string, object>();
            var constraintKeys = _route.Constraints.Keys.Select(k => k).ToList();
            foreach (var constraintKey in constraintKeys)
            {
                var constraint = _route.Constraints[constraintKey];
                var constraintToTest = constraint is IOptionalRouteConstraint
                                           ? ((IOptionalRouteConstraint)constraint).Constraint
                                           : constraint;

                if (!(constraintToTest is IQueryStringRouteConstraint))
                {
                    continue;
                }

                queryStringConstraints.Add(constraintKey, constraint);
                _route.Constraints.Remove(constraintKey);
            }

            // Let the underlying route do its thing.
            var virtualPathData = fromBaseMethod();

            // Add the querystring constraints back in.
            foreach (var queryStringConstraint in queryStringConstraints)
            {
                _route.Constraints.Add(queryStringConstraint.Key, queryStringConstraint.Value);
            }

            return virtualPathData;
        }

        /// <summary>
        /// Gets the translated virtual path for this route.
        /// </summary>
        /// <typeparam name="TVirtualPathData">
        /// The type of virtual path data to be returned. 
        /// This varies based on whether the route is a
        /// System.Web.Routing.Route or System.Web.Http.Routing.HttpRoute.
        /// </typeparam>
        /// <param name="fromTranslation">A delegate that can get the TVirtualPathData from a translated route</param>
        /// <returns>Returns null if no translation is available.</returns>
        public TVirtualPathData GetTranslatedVirtualPath<TVirtualPathData>(Func<IAttributeRoute, TVirtualPathData> fromTranslation)
            where TVirtualPathData : class
        {
            if (_route.Translations == null)
            {
                return null;
            }

            var translations = _route.Translations.ToArray();
            if (!translations.Any())
            {
                return null;
            }

            var currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            // Try and get the language-culture translation, then fall back to language translation
            var translation = translations.FirstOrDefault(t => t.CultureName == currentCultureName)
                              ?? translations.FirstOrDefault(t => currentCultureName.StartsWith(t.CultureName));

            if (translation == null)
            {
                return null;
            }

            return fromTranslation(translation);
        }

        /// <summary>
        /// Performs lowercasing and appends trailing slash if this route is so configured.
        /// </summary>
        /// <param name="virtualPath">The current virtual path, after translation</param>
        /// <returns>The final virtual path</returns>
        public string GetFinalVirtualPath(string virtualPath)
        {
            /**
             * Lowercase urls.
             * NOTE: The initial lowercasing of all BUT url params occurs in RouteBuilder.CreateRouteUrl().
             * This is just a final lowercasing of the final, parameter-replaced url.
             */

            var lower = _route.UseLowercaseRoute.GetValueOrDefault(_configuration.UseLowercaseRoutes);
            var preserve = _route.PreserveCaseForUrlParameters.GetValueOrDefault(_configuration.PreserveCaseForUrlParameters);
            
            if (lower && !preserve)
            {
                virtualPath = TransformVirtualPathToLowercase(virtualPath);
            }

            /**
             * Append trailing slashes
             */

            var appendTrailingSlash = _route.AppendTrailingSlash.GetValueOrDefault(_configuration.AppendTrailingSlash);
            if (appendTrailingSlash)
            {
                virtualPath = AppendTrailingSlashToVirtualPath(virtualPath);
            }

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
            {
                path += "/";
            }

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