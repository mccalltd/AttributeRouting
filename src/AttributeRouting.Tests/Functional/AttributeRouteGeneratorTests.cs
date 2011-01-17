using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
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
            
            var routeGenerator = new RouteBuilder(configuration);

            Routes = routeGenerator.BuildAllRoutes();

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