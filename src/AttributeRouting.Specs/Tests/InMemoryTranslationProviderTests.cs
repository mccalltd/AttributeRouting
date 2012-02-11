using System.Collections.Generic;
using System.Web.Routing;
using AttributeRouting.Framework.Localization;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests
{
    public class InMemoryTranslationProviderTests
    {
        [Test]
        public void Usage()
        {
            var translations = new InMemoryTranslationProvider();
                
            translations.Configure()
                .ByKey("HomePage_RouteUrl", new Dictionary<string, string>
                {
                    { "es", "spanish-home" },
                    { "fr", "french-home" }
                })
                .ByKey("AboutPage_RouteUrl", new Dictionary<string, string>
                {
                    { "es", "spanish-about" },
                    { "fr", "french-about" }
                });

            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.TranslationProvider = new InMemoryTranslationProvider();
            });

            Assert.That(translations.Translate("HomePage_RouteUrl", "es"), Is.EqualTo("spanish-home"));
            Assert.That(translations.Translate("AboutPage_RouteUrl", "fr"), Is.EqualTo("french-about"));
        }
    }
}
