using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AttributeRouting.Framework.Localization
{
    /// <summary>
    /// Fluent helper for adding translations for the route components of a controller in a strongly typed manner.
    /// </summary>
    /// <typeparam name="TController">The type of controler for which to add translations</typeparam>
    public class ControllerTranslationBuilder<TController>
    {
        private readonly TranslationBuilder _builder;
        private readonly TranslationKeyGenerator _keyGenerator;

        /// <summary>
        /// Fluent helper for adding translations for the route components of a controller.
        /// </summary>
        /// <param name="builder">The translation builder to use to utlimately add translations to the <see cref="FluentTranslationProvider"/></param>
        public ControllerTranslationBuilder(TranslationBuilder builder)
        {
            _builder = builder;
            _keyGenerator = new TranslationKeyGenerator();
        }

        /// <summary>
        /// Add translations for the area url specified via the <see cref="RouteAreaAttribute"/> applied to this controller.
        /// </summary>
        /// <param name="cultureTranslationPairs">Dictionary using cultureName as a key and a translation as the value</param>
        public ControllerTranslationBuilder<TController> AreaUrl(Dictionary<string, string> cultureTranslationPairs)
        {
            var key = _keyGenerator.AreaUrl<TController>();
            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }

        /// <summary>
        /// Add translations for the route prefix url specified via the <see cref="RoutePrefixAttribute"/> applied to this controller.
        /// </summary>
        /// <param name="cultureTranslationPairs">Dictionary using cultureName as a key and a translation as the value</param>
        public ControllerTranslationBuilder<TController> RoutePrefixUrl(Dictionary<string, string> cultureTranslationPairs)
        {
            var key = _keyGenerator.RoutePrefixUrl<TController>();
            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }

        /// <summary>
        /// Add translations for the route url specified via the route attribute applied to the specified action in this controller.
        /// </summary>
        /// <param name="action">Expression pointing to an action method on the controller</param>
        /// <param name="cultureTranslationPairs">Dictionary using cultureName as a key and a translation as the value</param>
        public ControllerTranslationBuilder<TController> RouteUrl(Expression<Func<TController, object>> action, Dictionary<string, string> cultureTranslationPairs)
        {
            var key = _keyGenerator.RouteUrl(action);
            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }
    }
}