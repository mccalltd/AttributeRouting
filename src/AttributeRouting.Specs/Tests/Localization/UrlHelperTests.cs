using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Framework.Localization;
using AttributeRouting.Web.Mvc;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Localization
{
    public class UrlHelperTests
    {
        [Test]
        public void It_returns_translated_routes()
        {
            var translations = new FluentTranslationProvider();

            translations.AddTranslations().ForController<TranslationController>()
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

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<TranslationController>();
                config.AddRoutesFromController<TranslationWithCustomKeysController>();
                config.AddTranslationProvider(translations);
            });

            var requestContext = MockBuilder.BuildRequestContext();

            // Default culture
            var urlHelper = new UrlHelper(requestContext, RouteTable.Routes);
            Assert.That(urlHelper.Action("Index", "Translation", new { area = "Area" }),
                        Is.EqualTo("/Area/Prefix/Index"));

            // es-ES culture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
            Assert.That(urlHelper.Action("Index", "Translation", new { area = "Area" }),
                        Is.EqualTo("/es-Area/es-Prefix/es-Index"));

            // es culture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
            Assert.That(urlHelper.Action("Index", "Translation", new { area = "Area" }),
                        Is.EqualTo("/es-Area/es-Prefix/es-Index"));
        }
    }
}