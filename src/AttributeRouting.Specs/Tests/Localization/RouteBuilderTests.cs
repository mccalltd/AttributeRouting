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
                .RoutePrefixUrl(new Dictionary<string, string>
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

            var translatedRoute = RouteTable.Routes.Cast<AttributeRoute>().SingleOrDefault(r => (string)r.DataTokens["cultureName"] == "es");
            Assert.That(translatedRoute, Is.Not.Null);
            Assert.That(translatedRoute.Url, Is.EqualTo("es-Area/es-Prefix/es-Index"));

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);
        }

        [Test]
        public void RouteBuilder_adds_partially_translated_route_to_route_table_when_only_parts_of_the_route_are_translated()
        {
            var translations = new FluentTranslationProvider();

            translations.AddTranslations().ForController<TranslationController>()
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

            var translatedRoute = RouteTable.Routes.Cast<AttributeRoute>().SingleOrDefault(r => (string)r.DataTokens["cultureName"] == "es");
            Assert.That(translatedRoute, Is.Not.Null);
            Assert.That(translatedRoute.Url, Is.EqualTo("Area/Prefix/es-Index"));

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);
        }
    }
}