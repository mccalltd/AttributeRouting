using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Moq;

namespace AttributeRouting.Specs.Tests
{
    public static class MockBuilder
    {
        public static Mock<HttpContextBase> BuildMockHttpContext(Action<Mock<HttpRequestBase>> requestMockConfig = null)
        {
            var requestMock = new Mock<HttpRequestBase>(MockBehavior.Strict);
            requestMock.SetupGet(x => x.AppRelativeCurrentExecutionFilePath).Returns("~/");
            requestMock.SetupGet(x => x.PathInfo).Returns("");
            requestMock.SetupGet(x => x.ApplicationPath).Returns("/");
            requestMock.SetupGet(x => x.Url).Returns(new Uri("http://localhost/", UriKind.Absolute));
            requestMock.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());
            requestMock.SetupGet(x => x.Headers).Returns(new NameValueCollection());
            requestMock.SetupGet(x => x.Form).Returns(new NameValueCollection());
            requestMock.SetupGet(x => x.QueryString).Returns(new NameValueCollection());
            requestMock.SetupGet(x => x.HttpMethod).Returns("GET");
            if (requestMockConfig != null)
                requestMockConfig(requestMock);

            var responseMock = new Mock<HttpResponseBase>(MockBehavior.Strict);
            responseMock.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);

            var httpContextMock = new Mock<HttpContextBase>(MockBehavior.Strict);
            httpContextMock.SetupGet(x => x.Request).Returns(requestMock.Object);
            httpContextMock.SetupGet(x => x.Response).Returns(responseMock.Object);

            return httpContextMock;
        }

        public static RequestContext BuildRequestContext()
        {
            var httpContextMock = BuildMockHttpContext();

            return new RequestContext(httpContextMock.Object, new RouteData());
        }
    }
}
