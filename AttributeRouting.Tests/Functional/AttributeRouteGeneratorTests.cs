using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AttributeRouting.Tests.Subjects.Controllers;
using NUnit.Framework;

namespace AttributeRouting.Tests.Functional.AttributeRouteGeneratorTests
{
    public class when_generating_routes
    {
        protected IEnumerable<AttributeRoute> Routes;

        [TestFixtureSetUp]
        public void test_fixture_setup()
        {
            var configuration = CreateConfiguration();
            
            var routeGenerator = new AttributeRoutesGenerator(configuration);

            Routes = routeGenerator.Generate();

            SetUp();
        }

        protected virtual void SetUp() { }

        protected virtual AttributeRoutingConfiguration CreateConfiguration()
        {
            var configuration = new AttributeRoutingConfiguration();
            
            configuration.ScanAssemblyOf<TestController>();

            return configuration;
        }

        protected AttributeRoute FetchRoute(string controller, string action)
        {
            return FetchRoutes(controller, action).Single();
        }

        protected IEnumerable<AttributeRoute> FetchRoutes(string controller, string action)
        {
            return from r in Routes
                   where (string)r.Defaults["action"] == action &&
                         (string)r.Defaults["controller"] == controller
                   select r;
        }

        protected string FetchHttpMethodForRoute(AttributeRoute route)
        {
            return (from c in route.Constraints
                    where c.Key == "httpMethod"
                    select ((RestfulHttpMethodConstraint)c.Value).AllowedMethods.Single()).Single();
        }
    }

    public class when_generating_the_test_controller_routes : when_generating_routes
    {
        [Test]
        public void the_output_is()
        {
            Routes.LogTo(Console.Out);
        }

        [Test]
        public void it_creates_17_routes()
        {
            Routes.Count().ShouldEqual(17);
        }
    }

    public class when_generating_routes_for_an_action_method : when_generating_routes
    {
        private AttributeRoute _indexAction;

        protected override void SetUp()
        {
            _indexAction = FetchRoute("RestfulTest", "Index");
        }

        [Test]
        public void the_url_is_equal_to_the_url_defined_in_the_route_attribute()
        {
            _indexAction.Url.ShouldEqual("Resources");
        }

        [Test]
        public void the_default_for_controller_is_equal_to_the_name_of_the_controller()
        {
            _indexAction.Defaults["controller"].ShouldEqual("RestfulTest");
        }

        [Test]
        public void the_default_for_action_is_equal_to_the_name_of_the_action_method()
        {
            _indexAction.Defaults["action"].ShouldEqual("Index");
        }

        [Test]
        public void the_namespaces_has_one_value_equal_to_the_controller_namespace()
        {
            var singleNamespace = ((string[])_indexAction.DataTokens["namespaces"]).Single();
            singleNamespace.ShouldEqual(typeof(RestfulTestController).Namespace);
        }
    }

    public class when_generating_routes_using_the_get_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("RestfulTest", "Index");
        }

        [Test]
        public void the_route_is_constrained_to_get_requests()
        {
            FetchHttpMethodForRoute(_route).ShouldEqual("GET");
        }
    }

    public class when_generating_routes_using_the_put_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("RestfulTest", "Create");
        }

        [Test]
        public void the_route_is_constrained_to_put_requests()
        {
            FetchHttpMethodForRoute(_route).ShouldEqual("PUT");
        }
    }

    public class when_generating_routes_using_the_post_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("RestfulTest", "Update");
        }

        [Test]
        public void the_route_is_constrained_to_post_requests()
        {
            FetchHttpMethodForRoute(_route).ShouldEqual("POST");
        }
    }

    public class when_generating_routes_using_the_delete_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("RestfulTest", "Destroy");
        }

        [Test]
        public void the_route_is_constrained_to_delete_requests()
        {
            FetchHttpMethodForRoute(_route).ShouldEqual("DELETE");
        }
    }

    public class when_generating_routes_using_the_route_area_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("AreaTest", "Index");
        }

        [Test]
        public void the_route_includes_the_area_first_in_the_url()
        {
            _route.Url.ShouldEqual("Area/Test");
        }

        [Test]
        public void the_route_has_a_data_token_for_the_area()
        {
            _route.DataTokens["area"].ShouldEqual("Area");
        }

        [Test]
        public void the_route_has_a_data_token_disabling_namespace_fallback()
        {
            _route.DataTokens["UseNamespaceFallback"].ShouldBeFalse();
        }
    }

    public class when_generating_an_explicit_area_route_in_a_controller_decorated_with_the_route_area_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("AreaTest", "DuplicateArea");
        }

        [Test]
        public void the_area_is_not_duplicated_in_the_url()
        {
            _route.Url.ShouldEqual("Area/DuplicateArea");
        }
    }

    public class when_generating_routes_using_the_route_prefix_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("NestedTest", "Index");
        }

        [Test]
        public void the_route_includes_the_prefix_in_the_url()
        {
            _route.Url.ShouldEqual("Parent/{parentId}/Child");
        }
    }

    public class when_generating_an_explicitly_prefixed_route_in_a_controller_decorated_with_the_route_prefix_attribute : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("NestedTest", "DuplicateNest");
            _route.LogTo(Console.Out);
        }

        [Test]
        public void the_prefix_is_not_duplicated_in_the_url()
        {
            _route.Url.ShouldEqual("Parent/{parentId}/DuplicateNest");
        }
    }

    public class when_generating_routes_using_both_the_route_area_and_route_prefix_attributes : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("NestedAreaTest", "Index");
            _route.LogTo(Console.Out);
        }

        [Test]
        public void the_route_url_has_the_area_first()
        {
            _route.Url.StartsWith("Area").ShouldBeTrue();
        }

        [Test]
        public void the_route_url_has_the_prefix_second()
        {
            _route.Url.StartsWith("Area/Parent/{parentId}").ShouldBeTrue();
        }
    }

    public class when_generating_routes_using_a_route_default : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("Test", "Default");
        }

        [Test]
        public void the_route_has_a_default_for_the_specified_key()
        {
            _route.Defaults["param1"].ShouldEqual("mapleleaf");
        }
    }

    public class when_generating_routes_using_a_route_constraint : when_generating_routes
    {
        private AttributeRoute _route;

        protected override void SetUp()
        {
            _route = FetchRoute("Test", "Constraint");
        }

        [Test]
        public void the_route_has_a_constraint_for_the_specified_key()
        {
            _route.Constraints["cat"].ShouldNotBeNull();
        }
    }

    public class when_generating_multiple_routes_for_a_single_action_method : when_generating_routes
    {
        private List<AttributeRoute> _routes;

        protected override void SetUp()
        {
            _routes = FetchRoutes("Test", "MultipleRoutes").ToList();
        }

        [Test]
        public void the_routes_are_generated_according_to_their_specified_order()
        {
            _routes[0].Url.ShouldEqual("Test/Multiple");
            _routes[1].Url.ShouldEqual("Test/Multiple/Routes");
            _routes[2].Url.ShouldEqual("Test/Multiple/Routes/Again");
        }
    }

    public class when_generating_routes_using_defaults_and_constraints_on_an_action_method_with_multiple_route_attributes : when_generating_routes
    {
        private List<AttributeRoute> _routes;

        protected override void SetUp()
        {
            _routes = FetchRoutes("Test", "MultipleRoutesWithDefaultsAndConstraints").ToList();
        }

        [Test]
        public void the_defaults_are_applied_only_to_the_specified_route()
        {
            _routes[0].Defaults["number"].ShouldEqual(666);
            _routes[1].Defaults["number"].ShouldEqual(777);
        }

        [Test]
        public void the_constraints_are_applied_only_to_the_specified_route()
        {
            ((RegexRouteConstraint)_routes[0].Constraints["number"]).Pattern.ShouldEqual(@"^\d{4}$");
            ((RegexRouteConstraint)_routes[1].Constraints["number"]).Pattern.ShouldEqual(@"^\d{1}$");
        }
    }

    public class when_generating_routes_using_configuration_to_specify_route_precendence : when_generating_routes
    {
        protected override AttributeRoutingConfiguration CreateConfiguration()
        {
            var configuration = new AttributeRoutingConfiguration();
            
            configuration.AddRoutesFromController<TestController>();
            configuration.AddRoutesFromController<RestfulTestController>();
            configuration.AddRoutesFromController<NestedTestController>();

            return configuration;
        }

        protected override void SetUp()
        {
            Routes.LogTo(Console.Out);
        }

        [Test]
        public void the_routes_for_each_controller_are_created_according_to_the_order_specified()
        {
            var routesAfterTestController = Routes.SkipWhile(r => r.Defaults["controller"].Equals("Test"));
            routesAfterTestController.Any(r => r.Defaults["controller"].Equals("Test")).ShouldBeFalse();

            var routesAfterRestfulTestController = routesAfterTestController.SkipWhile(r => r.Defaults["controller"].Equals("RestfulTest"));
            routesAfterRestfulTestController.Any(r => r.Defaults["controller"].Equals("RestfulTest")).ShouldBeFalse();
        }
    }

    public class when_generating_routes_using_configuration_to_specify_route_precendence_from_a_base_controller : when_generating_routes
    {
        protected override AttributeRoutingConfiguration CreateConfiguration()
        {
            var configuration = new AttributeRoutingConfiguration();
            configuration.AddRoutesFromControllersOfType<TestBaseController>();
            configuration.AddRoutesFromController<TestController>();

            return configuration;
        }

        protected override void  SetUp()
        {
            Routes.LogTo(Console.Out);
        }

        [Test]
        public void the_routes_for_the_derived_controllers_are_created_according_to_the_order_specified()
        {
            var routesAfterTestBaseController = Routes.SkipWhile(r => r.Defaults["controller"].Equals("DerivedTest"));
            routesAfterTestBaseController.Any(r => r.Defaults["controller"].Equals("DerivedTest")).ShouldBeFalse();
        }
    }
}