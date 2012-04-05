using System.Collections.Generic;
using System.Linq;

namespace AttributeRouting.Framework.Localization
{
    /// <summary>
    /// Default implementation of <see cref="TranslationProviderBase"/>
    /// allowing the addition of translations for route components in a fluent style.
    /// </summary>
    public class FluentTranslationProvider<TConstraint, TController> : TranslationProviderBase<TConstraint>
    {
        /// <summary>
        /// Default implementation of <see cref="TranslationProviderBase"/>
        /// allowing the addition of translations for route components in a fluent style.
        /// </summary>
        public FluentTranslationProvider()
        {
            Translations = new TranslationsDictionary();
        }

        /// <summary>
        /// Dictionary containing the translations of route components.
        /// </summary>
        public TranslationsDictionary Translations { get; private set; }

        public override IEnumerable<string> CultureNames
        {
            get
            {
                return (from key in Translations.Keys
                        from cultureName in Translations[key].Keys
                        select cultureName).Distinct();
            }
        }

        public override string GetTranslation(string key, string cultureName)
        {
            IDictionary<string, string> translationsByKey;
            if (!Translations.TryGetValue(key, out translationsByKey))
                return null;

            string translationByCulture;
            if (!translationsByKey.TryGetValue(cultureName, out translationByCulture))
                return null;

            return translationByCulture;
        }

        /// <summary>
        /// Returns a <see cref="TranslationBuilder"/> for adding translations in a fluent style.
        /// </summary>
        public TranslationBuilder AddTranslations()
        {
            return new TranslationBuilder(Translations);
        }
    }
}
