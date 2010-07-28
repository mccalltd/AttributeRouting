using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace AttributeRouting.Tests
{
    public static class TestHelper
    {
        public static MockRequestContext<TController> CreateMockRequestContext<TController>()
            where TController : Controller, new()
        {
            var mockHttpRequest = new Mock<HttpRequestBase>();
            var mockHttpResponse = new Mock<HttpResponseBase>();
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);

            var routeData = new RouteData();
            var controller = new TController();
            
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, routeData, controller);
            
            return new MockRequestContext<TController>
            {
                MockHttpRequest = mockHttpRequest,
                MockHttpResponse = mockHttpResponse,
                MockHttpContext = mockHttpContext,
                Controller = controller
            };
        }
    }

    public class MockRequestContext<TController>
        where TController : Controller
    {
        public Mock<HttpRequestBase> MockHttpRequest { get; set; }
        public Mock<HttpResponseBase> MockHttpResponse { get; set; }
        public Mock<HttpContextBase> MockHttpContext { get; set; }
        public TController Controller { get; set; }
    }
}
