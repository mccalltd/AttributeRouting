using System.Collections.Generic;
using System.Linq;

namespace AttributeRouting.Framework.Localization
{
    public class FluentTranslationProvider : TranslationProviderBase
    {
        public FluentTranslationProvider()
        {
            Translations = new TranslationsDictionary();
        }

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

        public override string Translate(string key, string culture)
        {
            IDictionary<string, string> translationsByKey;
            if (!Translations.TryGetValue(key, out translationsByKey))
                return null;

            string translationByCulture;
            if (!translationsByKey.TryGetValue(culture, out translationByCulture))
                return null;

            return translationByCulture;
        }

        public TranslationBuilder AddTranslations()
        {
            return new TranslationBuilder(Translations);
        }
    }
}
