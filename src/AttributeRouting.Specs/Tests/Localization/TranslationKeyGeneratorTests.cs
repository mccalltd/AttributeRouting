using AttributeRouting.Framework.Localization;
using AttributeRouting.Specs.Subjects;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Localization
{
    public class TranslationKeyGeneratorTests
    {
        [Test]
        public void It_returns_correct_keys_for_route_components()
        {
            var keyGenerator = new TranslationKeyGenerator();

            Assert.That(keyGenerator.AreaUrl<TranslationController>(), Is.EqualTo("Area_AreaUrl"));
           
            Assert.That(keyGenerator.RoutePrefixUrl<TranslationController>(),
                        Is.EqualTo("Area_Translation_RoutePrefixUrl"));
            
            Assert.That(keyGenerator.RouteUrl<TranslationController>(c => c.Index()),
                        Is.EqualTo("Area_Translation_Index_RouteUrl"));
        }
    }
}