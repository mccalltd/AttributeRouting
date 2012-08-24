using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Routing;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Logging;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Specs.Subjects.Http;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Logging;
using AttributeRouting.Web.Mvc;
using MvcContrib.TestHelper;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests
{
    public class BugFixTests
    {
        [Test]
        public void Generating_two_routes_for_api_get_requests()
        {
            // re: issue #102

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapHttpAttributeRoutes(config => config.AddRoutesFromController<Issue102TestController>());

            var routes = RouteTable.Routes.Cast<Route>().ToList();
            
            Assert.That(routes.Count, Is.EqualTo(2));
        }

        [Test]
        public void Ensure_that_incompletely_mocked_request_context_does_not_generate_error_in_determining_http_method()
        {
            // re: issue #25

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config => config.AddRoutesFromController<StandardUsageController>());

            "~/Index"
                .ShouldMapTo<StandardUsageController>(
                    x => x.Index());
        }

        [Test]
        public void Ensure_that_routes_with_optional_url_params_are_correctly_matched()
        {
            // re: issue #43

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config => config.AddRoutesFromController<BugFixesController>());

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);

            "~/BugFixes/Gallery/_CenterImage"
                .ShouldMapTo<BugFixesController>(
                    x => x.Issue43_OptionalParamsAreMucky(null, null, null, null));
        }

        [Test]
        public void Ensure_that_inbound_routing_works_when_contraining_by_culture()
        {
            // re: issue #53

            var translations = new FluentTranslationProvider();
            translations.AddTranslations().ForController<CulturePrefixController>().RouteUrl(x => x.Index(), new Dictionary<String, String> { { "pt", "Inicio" } });
            translations.AddTranslations().ForController<CulturePrefixController>().RouteUrl(x => x.Index(), new Dictionary<String, String> { { "en", "Home" } });

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<CulturePrefixController>();
                config.AddTranslationProvider(translations);
                config.ConstrainTranslatedRoutesByCurrentUICulture = true;
                config.CurrentUICultureResolver = (httpContext, routeData) =>
                {
                    return (string)routeData.Values["culture"]
                           ?? Thread.CurrentThread.CurrentUICulture.Name;
                };
            });

            RouteTable.Routes.Cast<Route>().LogTo(Console.Out);

            "~/en/cms/home".ShouldMapTo<CulturePrefixController>(x => x.Index());
            Assert.That("~/en/cms/inicio".Route(), Is.Null);
            Assert.That("~/pt/cms/home".Route(), Is.Null);
            "~/pt/cms/inicio".ShouldMapTo<CulturePrefixController>(x => x.Index());
        }

		[Test]
		public void Ensure_that_async_controller_action_can_be_mapped()
		{
			// re: issue #84
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapAttributeRoutes(config => config.AddRoutesFromController<AsyncActionController>());

			"~/WithAsync/Synchronous".ShouldMapTo<AsyncActionController>(x => x.Test1());
			var asyncRouteData = "~/WithAsync/NotSynchronous".Route();
			asyncRouteData.Values["controller"].ShouldEqual("AsyncAction", "Asynchronous route does not map to the AsyncActionController.");
			asyncRouteData.Values["action"].ShouldEqual("Test2", "Asynchronous route does not map to the correct action method.");
		}
    }
}
