using System.Collections.Generic;

namespace AttributeRouting.Framework.Localization
{
    public abstract class TranslationProviderBase
    {
        private TranslationKeyGenerator _keyGenerator;

        protected TranslationProviderBase()
        {
            _keyGenerator = new TranslationKeyGenerator();
        }

        public abstract IEnumerable<string> CultureNames { get; }

        public abstract string Translate(string key, string culture);

        internal string TranslateAreaUrl(string cultureName, RouteSpecification routeSpec)
        {
            var key = routeSpec.AreaUrlTranslationKey
                      ?? _keyGenerator.AreaUrl(routeSpec.AreaName);

            return Translate(key, cultureName);
        }

        internal string TranslateRoutePrefix(string cultureName, RouteSpecification routeSpec)
        {
            var key = routeSpec.RoutePrefixUrlTranslationKey
                      ?? _keyGenerator.RoutePrefixUrl(routeSpec.AreaName, routeSpec.ControllerName);

            return Translate(key, cultureName);
        }

        internal string TranslateRouteUrl(string cultureName, RouteSpecification routeSpec)
        {
            var key = routeSpec.RouteUrlTranslationKey
                      ?? _keyGenerator.RouteUrl(routeSpec.AreaName, routeSpec.ControllerName, routeSpec.ActionName);

            return Translate(key, cultureName);
        }
    }
}