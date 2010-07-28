using System;
using System.Web.Routing;
using AttributeRouting.Tests.Subjects.Controllers;
using NUnit.Framework;

namespace AttributeRouting.Tests.Unit.AttributeRouteTests
{
    public class when_configured_to_generate_lowercase_routes
    {
        [Test]
        public void it_transforms_mixed_case_to_lowercase()
        {
            const string url = "MixedCasE";

            var mockRequestContext = TestHelper.CreateMockRequestContext<TestController>();
            mockRequestContext.MockHttpRequest.Setup(x => x.Url).Returns(new Uri("http://fake.domain.com/" + url));

            var route = new AttributeRoute(url, null, null, null, true);

            var requestContext = mockRequestContext.Controller.ControllerContext.RequestContext;

            var virtualPathData = route.GetVirtualPath(requestContext, new RouteValueDictionary());

            virtualPathData.ShouldNotBeNull();
            virtualPathData.VirtualPath.ShouldEqual(url.ToLowerInvariant());
        }
    }
}
