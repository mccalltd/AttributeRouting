using System.Collections.Generic;

namespace AttributeRouting.Framework.Localization
{
    public class InMemoryTranslationProvider : ITranslationProvider
    {
        public InMemoryTranslationProvider()
        {
            Translations = new TranslationsCollection();
        }

        public TranslationsCollection Translations { get; private set; }

        public InMemoryTranslationBuilder Configure()
        {
            return new InMemoryTranslationBuilder(Translations);
        }

        public string Translate(string key, string culture)
        {
            IDictionary<string, string> translationsByKey;
            if (!Translations.TryGetValue(key, out translationsByKey))
                return null;

            string translationByCulture;
            if (!translationsByKey.TryGetValue(culture, out translationByCulture))
                return null;

            return translationByCulture;
        }
    }
}
