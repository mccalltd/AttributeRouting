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

        public string GetAreaUrl(string cultureName, RouteSpecification routeSpec)
        {
            var key = _keyGenerator.AreaUrl(routeSpec.AreaName);
            return Translate(key, cultureName);
        }

        public string GetRoutePrefix(string cultureName, RouteSpecification routeSpec)
        {
            var key = _keyGenerator.RoutePrefix(routeSpec.AreaName, routeSpec.ControllerName);
            return Translate(key, cultureName);
        }

        public string GetRouteUrl(string cultureName, RouteSpecification routeSpec)
        {
            var key = _keyGenerator.RouteUrl(routeSpec.AreaName, routeSpec.ControllerName, routeSpec.ActionName);
            return Translate(key, cultureName);
        }
    }
}