using System;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Logging;
using AttributeRouting.Specs.Subjects;
using MvcContrib.TestHelper;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests
{
    public class BugFixTests
    {
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
    }
}
