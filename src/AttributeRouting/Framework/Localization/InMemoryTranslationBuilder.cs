using System.Collections.Generic;

namespace AttributeRouting.Framework.Localization
{
    public class InMemoryTranslationBuilder
    {
        private readonly InMemoryTranslations _translations;

        public InMemoryTranslationBuilder(InMemoryTranslations translations)
        {
            _translations = translations;
        }

        public InMemoryTranslationBuilder ByKey(string key, Dictionary<string, string> cultureTranslationPairs)
        {
            IDictionary<string, string> translationsByKey;
            if (!_translations.TryGetValue(key, out translationsByKey))
                _translations.Add(key, cultureTranslationPairs);

            return this;
        }

    }
}