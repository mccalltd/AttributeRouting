using System.Web.Routing;
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
            RouteTable.Routes.MapAttributeRoutes();

            "~/Index".ShouldMapTo<StandardUsageController>(x => x.Index());
        }
    }
}
