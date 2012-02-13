using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Logging;
using AttributeRouting.Specs.Subjects;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Localization
{
    public class RouteBuilderTests
    {
        [Test]
        public void RouteBuilder_adds_translated_routes_to_route_table()
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

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<TranslationController>();
                config.TranslationProvider = translations;
            });

            // Ensure that a route is added for each translation
            Assert.That(RouteTable.Routes.Count, Is.EqualTo(2));

            var translatedRoute = RouteTable.Routes.SingleOrDefault(r => (string)((AttributeRoute)r).DataTokens["cultureName"] == "es");
            Assert.That(translatedRoute, Is.Not.Null);

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);
        }
    }
}