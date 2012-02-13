using System.Collections.Generic;

namespace AttributeRouting.Framework.Localization
{
    /// <summary>
    /// Provider for generating translations of route components.
    /// </summary>
    public abstract class TranslationProviderBase
    {
        private readonly TranslationKeyGenerator _keyGenerator;

        protected TranslationProviderBase()
        {
            _keyGenerator = new TranslationKeyGenerator();
        }

        /// <summary>
        /// List of culture names that have translations available via this provider.
        /// </summary>
        public abstract IEnumerable<string> CultureNames { get; }

        /// <summary>
        /// Gets the translation for the given route component key and culture.
        /// </summary>
        /// <param name="key">The key of the route component to translate; see <see cref="TranslationKeyGenerator"/></param>
        /// <param name="cultureName">The culture name for the translation</param>
        public abstract string GetTranslation(string key, string cultureName);

        internal string TranslateAreaUrl(string cultureName, RouteSpecification routeSpec)
        {
            var key = routeSpec.AreaUrlTranslationKey
                      ?? _keyGenerator.AreaUrl(routeSpec.AreaName);

            return GetTranslation(key, cultureName);
        }

        internal string TranslateRoutePrefix(string cultureName, RouteSpecification routeSpec)
        {
            var key = routeSpec.RoutePrefixUrlTranslationKey
                      ?? _keyGenerator.RoutePrefixUrl(routeSpec.AreaName, routeSpec.ControllerName);

            return GetTranslation(key, cultureName);
        }

        internal string TranslateRouteUrl(string cultureName, RouteSpecification routeSpec)
        {
            var key = routeSpec.RouteUrlTranslationKey
                      ?? _keyGenerator.RouteUrl(routeSpec.AreaName, routeSpec.ControllerName, routeSpec.ActionName);

            return GetTranslation(key, cultureName);
        }
    }
}