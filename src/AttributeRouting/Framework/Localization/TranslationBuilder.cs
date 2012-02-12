using System.Collections.Generic;
using System.Web.Mvc;

namespace AttributeRouting.Framework.Localization
{
    public class TranslationBuilder
    {
        private readonly TranslationsDictionary _translations;

        public TranslationBuilder(TranslationsDictionary translations)
        {
            _translations = translations;
        }

        public TranslationBuilder ForKey(string key, Dictionary<string, string> cultureTranslationPairs)
        {
            IDictionary<string, string> translationsByKey;
            if (!_translations.TryGetValue(key, out translationsByKey))
                _translations.Add(key, cultureTranslationPairs);

            return this;
        }

        public ControllerTranslationBuilder<TController> ForController<TController>()
            where TController : IController
        {
            return new ControllerTranslationBuilder<TController>(this);
        }
    }
}