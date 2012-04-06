using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Framework.Localization;
using AttributeRouting.Web.Logging;
using AttributeRouting.Web.Mvc;
using AttributeRouting.Web.Mvc.Framework;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Localization
{
    public class RouteBuilderTests
    {
        [Test]
        public void It_adds_translated_routes_to_route_table()
        {
            var translationProvider = new FluentTranslationProvider();

            translationProvider.AddTranslations().ForController<TranslationController>()
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
                config.AddTranslationProvider(translationProvider);
            });

            // Ensure that a route is added for each translation
            Assert.That(RouteTable.Routes.Count, Is.EqualTo(2));

            var translatedRoute = RouteTable.Routes.Cast<AttributeRoute>().SingleOrDefault(r => (string)r.DataTokens["cultureName"] == "es");
            Assert.That(translatedRoute, Is.Not.Null);
            Assert.That(translatedRoute.Url, Is.EqualTo("es-Area/es-Prefix/es-Index"));

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);
        }

        [Test]
        public void It_adds_partially_translated_route_to_route_table_when_only_parts_of_the_route_are_translated()
        {
            var translationProvider = new FluentTranslationProvider();

            translationProvider.AddTranslations().ForController<TranslationController>()
                .RouteUrl(c => c.Index(), new Dictionary<string, string>
                {
                    { "es", "es-Index" }
                });

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<TranslationController>();
                config.AddTranslationProvider(translationProvider);
            });

            // Ensure that a route is added for each translation
            Assert.That(RouteTable.Routes.Count, Is.EqualTo(2));

            var translatedRoute = RouteTable.Routes.Cast<AttributeRoute>().SingleOrDefault(r => (string)r.DataTokens["cultureName"] == "es");
            Assert.That(translatedRoute, Is.Not.Null);
            Assert.That(translatedRoute.Url, Is.EqualTo("Area/Prefix/es-Index"));

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);
        }
        
        [Test]
        public void It_will_mix_and_match_translations_suppied_by_all_providers_when_building_route_url()
        {
            var translationProvider1 = new FluentTranslationProvider();
            translationProvider1.AddTranslations().ForController<TranslationController>()
                .RouteUrl(c => c.Index(), new Dictionary<string, string>
                {
                    { "es", "es-Index" }
                });

            var translationProvider2 = new FluentTranslationProvider();
            translationProvider2.AddTranslations().ForController<TranslationController>()
                .RoutePrefixUrl(new Dictionary<string, string>
                {
                    { "es", "es-Prefix" }
                });

            var translationProvider3 = new FluentTranslationProvider();
            translationProvider3.AddTranslations().ForController<TranslationController>()
                .AreaUrl(new Dictionary<string, string>
                {
                    { "es", "es-Area" }
                });

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<TranslationController>();
                config.AddTranslationProvider(translationProvider1);
                config.AddTranslationProvider(translationProvider2);
                config.AddTranslationProvider(translationProvider3);
            });

            // Ensure that a route is added for each translation
            Assert.That(RouteTable.Routes.Count, Is.EqualTo(2));

            var translatedRoute = RouteTable.Routes.Cast<AttributeRoute>().SingleOrDefault(r => (string)r.DataTokens["cultureName"] == "es");
            Assert.That(translatedRoute, Is.Not.Null);
            Assert.That(translatedRoute.Url, Is.EqualTo("es-Area/es-Prefix/es-Index"));

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);
        }
    }
}