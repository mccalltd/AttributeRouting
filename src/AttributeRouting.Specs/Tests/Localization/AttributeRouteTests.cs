using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Specs.Subjects;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Localization
{
    public class AttributeRouteTests
    {
        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();    
        }

        [Test]
        public void It_matches_route_when_no_translations_are_available()
        {
            var routes = RouteTable.Routes;
            routes.MapAttributeRoutes(c => c.AddRoutesFromController<TranslateActionsController>());

            // Fetch the first route
            var route = routes.Cast<AttributeRoute>().SingleOrDefault();
            Assert.That(route, Is.Not.Null);

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns("Translate/Actions/Index");
            });
            
            var routeData = route.GetRouteData(httpContextMock.Object);
            Assert.That(routeData, Is.Not.Null);
        }

        [Test]
        public void It_matches_default_route_when_no_translations_are_available_for_neutral_culture()
        {
            var route = MapRoutesAndFetchFirst(r => r.CultureName == null);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns("Translate/Actions/Index");
            });
            
            var routeData = route.GetRouteData(httpContextMock.Object);
            Assert.That(routeData, Is.Not.Null);
        }

        [Test]
        public void It_does_not_match_default_route_when_translation_is_available_for_specific_culture()
        {
            var route = MapRoutesAndFetchFirst(r => r.CultureName == null);
            
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns("Translate/Actions/Index");
            });
            
            var routeData = route.GetRouteData(httpContextMock.Object);
            Assert.That(routeData, Is.Null);
        }

        [Test]
        public void It_does_not_match_default_route_when_translation_is_available_for_neutral_culture()
        {
            var route = MapRoutesAndFetchFirst(r => r.CultureName == null);
            
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-AR");

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns("Translate/Actions/Index");
            });
            
            var routeData = route.GetRouteData(httpContextMock.Object);
            Assert.That(routeData, Is.Null);
        }

        [Test]
        public void It_matches_translated_route_when_translation_is_available_for_culture()
        {
            var route = MapRoutesAndFetchFirst(r => r.CultureName == "es-ES");
            
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns("Translate/Actions/HOLA!");
            });
            
            var routeData = route.GetRouteData(httpContextMock.Object);
            Assert.That(routeData, Is.Not.Null);
        }

        [Test]
        public void It_matches_translated_route_for_neutral_culture_when_no_translation_is_available_for_specific_culture()
        {
            var route = MapRoutesAndFetchFirst(r => r.CultureName == "es");
            
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-AR");

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns("Translate/Actions/hola");
            });
            
            var routeData = route.GetRouteData(httpContextMock.Object);
            Assert.That(routeData, Is.Not.Null);
        }

        [Test]
        public void It_does_not_match_translated_route_for_neutral_culture_when_translation_is_available_for_specific_culture()
        {
            var route = MapRoutesAndFetchFirst(r => r.CultureName == "es");
            
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.PathInfo).Returns("Translate/Actions/hola");
            });
            
            var routeData = route.GetRouteData(httpContextMock.Object);
            Assert.That(routeData, Is.Null);
        }

        private AttributeRoute MapRoutesAndFetchFirst(Func<AttributeRoute, bool> predicate)
        {
            var provider = new FluentTranslationProvider();

            provider.AddTranslations().ForController<TranslateActionsController>()
                .RouteUrl(c => c.Index(1), new Dictionary<string, string>
                {
                    { "es", "hola" },
                    { "es-ES", "HOLA!" }
                });

            var routes = RouteTable.Routes;
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<TranslateActionsController>();
                config.AddTranslationProvider(provider);
                config.ConstrainTranslatedRoutesByCurrentUICulture = true;
            });

            // Fetch the first route
            var route = routes.Cast<AttributeRoute>().FirstOrDefault(predicate);
            Assert.That(route, Is.Not.Null);

            return route;
        }
    }
}