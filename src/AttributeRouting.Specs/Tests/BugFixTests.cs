using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Specs.Subjects.Http;
using AttributeRouting.Web.Http.Constraints;
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
        public void Issue218_Url_generation_with_optional_query_params()
        {
            // re: issue #218

            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<Issue218TestController>());
            RouteTable.Routes.LogTo(Console.Out);
            
            var urlHelper = new UrlHelper(MockBuilder.BuildRequestContext());

            Assert.That(urlHelper.Action("NoQuery", "Issue218Test", new { categoryId = 12 }),
                        Is.EqualTo("/Issue-218/No-Query?categoryId=12"));

            Assert.That(urlHelper.Action("OptionalQuery", "Issue218Test", new { categoryId = 12 }),
                        Is.EqualTo("/Issue-218/Optional-Query?categoryId=12"));

            Assert.That(urlHelper.Action("DefaultQuery", "Issue218Test"),
                        Is.EqualTo("/Issue-218/Default-Query?categoryId=123"));
        }

        [Test]
        public void Issue161_Querystring_param_constraints_mucks_up_url_generation()
        {
            // re: issue #161
            
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<Issue161TestController>());

            var urlHelper = new UrlHelper(MockBuilder.BuildRequestContext());
            var routeValues = new { area = "Cms", culture = "en", p = 1 };
            var expectedUrl = urlHelper.Action("Index", "Issue161Test", routeValues);

            Assert.That(expectedUrl, Is.EqualTo("/en/Cms/Content/Items?p=1"));
        }

        [Test]
        public void Issue120_OData_style_http_url_bonks()
        {
            // re: issue #120
            
            var httpRoutes = GlobalConfiguration.Configuration.Routes;
            httpRoutes.Clear();
            httpRoutes.MapHttpAttributeRoutes(config => config.AddRoutesFromController<HttpBugFixesController>());

            // Just make sure we don't get an exception
            Assert.That(httpRoutes.Count, Is.EqualTo(1));
        }

        [Test]
        public void Issue102_Generating_two_routes_for_api_get_requests()
        {
            // re: issue #102
            
            var httpRoutes = GlobalConfiguration.Configuration.Routes;
            httpRoutes.Clear();
            httpRoutes.MapHttpAttributeRoutes(config => config.AddRoutesFromController<Issue102TestController>());

            var routes = RouteTable.Routes.Cast<Route>().ToList();
            
            Assert.That(routes.Count, Is.EqualTo(2));
        }

        [Test]
        public void Issue25_Ensure_that_incompletely_mocked_request_context_does_not_generate_error_in_determining_http_method()
        {
            // re: issue #25

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config => config.AddRoutesFromController<StandardUsageController>());

            "~/Index"
                .ShouldMapTo<StandardUsageController>(
                    x => x.Index());
        }

        [Test]
        public void Issue43_Ensure_that_routes_with_optional_url_params_are_correctly_matched()
        {
            // re: issue #43

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config => config.AddRoutesFromController<BugFixesController>());

            RouteTable.Routes.LogTo(Console.Out);

            "~/BugFixes/Gallery/_CenterImage"
                .ShouldMapTo<BugFixesController>(
                    x => x.Issue43_OptionalParamsAreMucky(null, null, null, null));
        }

        [Test]
        public void Issue53_Ensure_that_inbound_routing_works_when_contraining_by_culture()
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

            RouteTable.Routes.LogTo(Console.Out);

            "~/en/cms/home".ShouldMapTo<CulturePrefixController>(x => x.Index());
            Assert.That("~/en/cms/inicio".Route(), Is.Null);
            Assert.That("~/pt/cms/home".Route(), Is.Null);
            "~/pt/cms/inicio".ShouldMapTo<CulturePrefixController>(x => x.Index());
        }

		[Test]
		public void Issue84_Ensure_that_async_controller_action_can_be_mapped()
		{
			// re: issue #84
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapAttributeRoutes(config => config.AddRoutesFromController<AsyncActionController>());

			"~/WithAsync/Synchronous".ShouldMapTo<AsyncActionController>(x => x.Test1());
			var asyncRouteData = "~/WithAsync/NotSynchronous".Route();
			asyncRouteData.Values["controller"].ShouldEqual("AsyncAction", "Asynchronous route does not map to the AsyncActionController.");
			asyncRouteData.Values["action"].ShouldEqual("Test2", "Asynchronous route does not map to the correct action method.");
		}

        [Test]
        public void Issue191_in_memory_config_initializes_routes_with_general_http_constraints()
        {
            var inMemoryConfig = new HttpConfiguration();

            inMemoryConfig.Routes.MapHttpAttributeRoutes(x =>
                {
                    x.InMemory = true;
                    x.AddRoutesFromController<HttpStandardUsageController>();
                });

            Assert.AreEqual(6, inMemoryConfig.Routes.Count);
            Assert.True(inMemoryConfig.Routes.All(x => x.Constraints.All(c => c.Value.GetType() == typeof(InboundHttpMethodConstraint))));
        }

        [Test]
        public void Issue191_default_web_config_initializes_routes_with_web_http_constraints()
        {
            var inMemoryConfig = GlobalConfiguration.Configuration;
            inMemoryConfig.Routes.Clear();

            inMemoryConfig.Routes.MapHttpAttributeRoutes(x => x.AddRoutesFromController<HttpStandardUsageController>());

            Assert.AreEqual(6, inMemoryConfig.Routes.Count);
            Assert.True(inMemoryConfig.Routes.All(x => x.Constraints.All(c => c.Value.GetType() == typeof(Web.Http.Constraints.InboundHttpMethodConstraint))));
        }

        [Test]
        public void Issue191_in_memory_web_config_inits_general_http_constraint_factory()
        {
            var inMemoryConfig = new HttpWebRouteConfiguration(inMemory:true);
            Assert.IsAssignableFrom<AttributeRouting.Web.Http.Framework.RouteConstraintFactory>(inMemoryConfig.RouteConstraintFactory);
        }

        [Test]
        public void Issue191_default_web_config_inits_web_http_constraint_factory()
        {
            var inMemoryConfig = new HttpWebRouteConfiguration();
            Assert.IsAssignableFrom<AttributeRouting.Web.Http.Framework.RouteConstraintFactory>(inMemoryConfig.RouteConstraintFactory);
        }
    }
}
