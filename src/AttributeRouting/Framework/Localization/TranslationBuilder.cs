using System.Collections.Generic;

namespace AttributeRouting.Framework.Localization
{
    /// <summary>
    /// Fluent helper for adding translations for route components.
    /// </summary>
    public class TranslationBuilder
    {
        private readonly TranslationsDictionary _translations;

        /// <summary>
        /// Fluent helper for adding translations for route components.
        /// </summary>
        public TranslationBuilder(TranslationsDictionary translations)
        {
            _translations = translations;
        }

        /// <summary>
        /// Adds translations to the <see cref="FluentTranslationProvider"/> 
        /// based on the key of the route component to translate; 
        /// see <see cref="TranslationKeyGenerator"/>
        /// </summary>
        public TranslationBuilder ForKey(string key, Dictionary<string, string> cultureTranslationPairs)
        {
            IDictionary<string, string> translationsByKey;
            if (!_translations.TryGetValue(key, out translationsByKey))
                _translations.Add(key, cultureTranslationPairs);

            return this;
        }
    }
}