using System.Collections.Generic;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Mvc.Framework.Localization;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Localization
{
    public class FluentTranslationProviderTests
    {
        [Test]
        public void It_can_add_tranlsations_for_actions_with_defaulted_params()
        {
            var provider = new FluentTranslationProvider();

            provider.AddTranslations().ForController<TranslateActionsController>()
                .RouteUrl(c => c.Index(1), new Dictionary<string, string>
                {
                    { "es", "hola" }
                });

            var keyGenerator = new TranslationKeyGenerator();
            var translationKey = keyGenerator.RouteUrl<TranslateActionsController>(c => c.Index(1));
            var translation = provider.GetTranslation(translationKey, "es");

            Assert.That(translation, Is.EqualTo("hola"));
        }

        [Test]
        public void It_returns_translation_for_given_keys()
        {
            var provider = new FluentTranslationProvider();

            provider.AddTranslations().ForController<TranslationController>()
                .AreaUrl(new Dictionary<string, string>
                {
                    { "es", "es-Area" }
                })
                .RoutePrefixUrl(new Dictionary<string, string>
                {
                    { "es", "es-Prefix" }
                })
                .RouteUrl(c => c.Index(), new Dictionary<string, string>
                {
                    { "es", "es-Index" }
                });

            provider.AddTranslations()
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

            Assert.That(provider.GetTranslation(keyGenerator.AreaUrl<TranslationController>(), "en"), Is.Null);
            Assert.That(provider.GetTranslation(keyGenerator.RoutePrefixUrl<TranslationController>(), "en"), Is.Null);
            Assert.That(provider.GetTranslation(keyGenerator.RouteUrl<TranslationController>(c => c.Index()), "en"), Is.Null);

            Assert.That(provider.GetTranslation(keyGenerator.AreaUrl<TranslationController>(), "es"), Is.EqualTo("es-Area"));
            Assert.That(provider.GetTranslation(keyGenerator.RoutePrefixUrl<TranslationController>(), "es"), Is.EqualTo("es-Prefix"));
            Assert.That(provider.GetTranslation(keyGenerator.RouteUrl<TranslationController>(c => c.Index()), "es"), Is.EqualTo("es-Index"));

            Assert.That(provider.GetTranslation("CustomAreaKey", "es"), Is.EqualTo("es-CustomArea"));
            Assert.That(provider.GetTranslation("CustomPrefixKey", "es"), Is.EqualTo("es-CustomPrefix"));
            Assert.That(provider.GetTranslation("CustomRouteKey", "es"), Is.EqualTo("es-CustomIndex"));
        }
    }
}