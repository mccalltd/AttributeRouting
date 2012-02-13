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
        public void GetVirtualPath_does_not_append_trailing_slash_to_urls_when_not_configured_to_do_so() {
            var route = BuildAttributeRoute("Controller/Action", false, false);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(route.Url));
        }

        [Test]
        public void GetVirtualPath_does_not_lowercase_urls_when_not_configured_to_do_so()
        {
            var route = BuildAttributeRoute("Controller/Action", false, false);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(route.Url));
        }

        [Test]
        public void GetVirtualPath_returns_lowercase_paths_when_configured_to_do_so()
        {
            var route = BuildAttributeRoute("Controller/Action", true, false);

            var routeValues = new RouteValueDictionary(new { query = "MixedCase" });
            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, routeValues);

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath,
                        Is.EqualTo(route.Url.ToLowerInvariant() + "?query=MixedCase"));
        }

        [Test]
        public void GetVirtualPath_returns_paths_with_trailing_slash_when_configured_to_do_so() {
            var route = BuildAttributeRoute("Controller/Action", false, true);

            var routeValues = new RouteValueDictionary(new { query = "MixedCase" });
            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, routeValues);

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath,
                        Is.EqualTo(route.Url + "/?query=MixedCase"));
        }

        [Test]
        public void GetVirtualPath_does_not_append_trailing_slahs_to_root_urls_when_configured_for_trailing_slashes() {
            var route = BuildAttributeRoute("", false, true);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(""));
        }

        private AttributeRoute BuildAttributeRoute(string url, bool useLowercaseRoutes, bool appendTrailingSlash)
        {
            var configuration = new AttributeRoutingConfiguration
            {
                UseLowercaseRoutes = useLowercaseRoutes,
                AppendTrailingSlash = appendTrailingSlash,
            };

            return new AttributeRoute(url,
                                      new RouteValueDictionary(),
                                      new RouteValueDictionary(),
                                      new RouteValueDictionary(),
                                      configuration);
        }
    }
}
