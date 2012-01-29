using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;
using Moq;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests
{
    public class AttributeRouteTests
    {
        private Mock<RequestContext> _requestContextMock;

        [SetUp]
        public void SetUp()
        {
            var httpContextMock = new Mock<HttpContextBase>();

            _requestContextMock = new Mock<RequestContext>();
            _requestContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            _requestContextMock.Setup(x => x.RouteData).Returns(new RouteData());
        }

        [Test]
        public void GetVirtualPath_does_not_lowercase_urls_when_not_configured_to_do_so()
        {
            var route = BuildAttributeRoute("Controller/Action", false);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(route.Url));
        }

        [Test]
        public void GetVirtualPath_returns_lowercase_paths_when_configured_to_do_so()
        {
            var route = BuildAttributeRoute("Controller/Action", true);

            var routeValues = new RouteValueDictionary(new { query = "MixedCase" });
            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, routeValues);

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath,
                        Is.EqualTo(route.Url.ToLowerInvariant() + "?query=MixedCase"));
        }

        private AttributeRoute BuildAttributeRoute(string url, bool useLowercaseRoutes)
        {
            return new AttributeRoute(null,
                                      url,
                                      new RouteValueDictionary(),
                                      new RouteValueDictionary(),
                                      new RouteValueDictionary(),
                                      useLowercaseRoutes,
                                      new MvcRouteHandler());
        }
    }
}
