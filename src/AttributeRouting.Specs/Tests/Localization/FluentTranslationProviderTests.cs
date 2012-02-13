using System.Collections.Generic;
using System.Linq;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Specs.Subjects;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Localization
{
    public class FluentTranslationProviderTests
    {
        [Test]
        public void FluentTranslationProvider_returns_translation_for_given_keys()
        {
            var translations = new FluentTranslationProvider();

            translations.AddTranslations().ForController<TranslationController>()
                .AreaUrl(new Dictionary<string, string>
                {
                    { "es", "es-Area" }
                })
                .RoutePrefix(new Dictionary<string, string>
                {
                    { "es", "es-Prefix" }
                })
                .RouteUrl(c => c.Index(), new Dictionary<string, string>
                {
                    { "es", "es-Index" }
                });

            translations.AddTranslations()
                .ForKey("CustomAreaKey", new Dictionary<string, string>
                {
                    { "es", "es-CustomArea" }
                })
                .ForKey("CustomPrefixKey", new Dictionary<string, string>
                {
                    { "es", "es-CustomPrefix" }
                })
                .ForKey("CustomRouteKey", new Dictionary<string, string>
                {
                    { "es", "es-CustomIndex" }
                });

            var keyGenerator = new TranslationKeyGenerator();

            Assert.That(translations.Translate(keyGenerator.AreaUrl<TranslationController>(), "en"), Is.Null);
            Assert.That(translations.Translate(keyGenerator.RoutePrefixUrl<TranslationController>(), "en"), Is.Null);
            Assert.That(translations.Translate(keyGenerator.RouteUrl<TranslationController>(c => c.Index()), "en"),
                        Is.Null);

            Assert.That(translations.Translate(keyGenerator.AreaUrl<TranslationController>(), "es"),
                        Is.EqualTo("es-Area"));
            Assert.That(translations.Translate(keyGenerator.RoutePrefixUrl<TranslationController>(), "es"),
                        Is.EqualTo("es-Prefix"));
            Assert.That(translations.Translate(keyGenerator.RouteUrl<TranslationController>(c => c.Index()), "es"),
                        Is.EqualTo("es-Index"));

            Assert.That(translations.Translate("CustomAreaKey", "es"), Is.EqualTo("es-CustomArea"));
            Assert.That(translations.Translate("CustomPrefixKey", "es"), Is.EqualTo("es-CustomPrefix"));
            Assert.That(translations.Translate("CustomRouteKey", "es"), Is.EqualTo("es-CustomIndex"));

            Assert.That(translations.CultureNames.Count(), Is.EqualTo(1));
            Assert.That(translations.CultureNames.First(), Is.EqualTo("es"));
        }
    }
}