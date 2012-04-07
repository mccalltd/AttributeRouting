using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Framework;
using AttributeRouting.Web.Mvc;
using AttributeRouting.Web.Mvc.Framework;
using Moq;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.TrailingSlashes
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
        public void It_does_not_append_trailing_slash_to_urls_when_not_configured_to_do_so() 
        {
            var route = BuildAttributeRoute("Controller/Action", false, false);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(route.Url));
        }

        [Test]
        public void It_returns_paths_with_trailing_slash_when_configured_to_do_so() 
        {
            var route = BuildAttributeRoute("Controller/Action", false, true);

            var routeValues = new RouteValueDictionary(new { query = "MixedCase" });
            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, routeValues);

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath,
                        Is.EqualTo(route.Url + "/?query=MixedCase"));
        }

        [Test]
        public void It_does_not_append_trailing_slahs_to_root_urls_when_configured_for_trailing_slashes() 
        {
            var route = BuildAttributeRoute("", false, true);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(""));
        }

        private Route BuildAttributeRoute(string url, bool useLowercaseRoutes, bool appendTrailingSlash)
        {
            var configuration = new AttributeRoutingConfiguration
            {
                UseLowercaseRoutes = useLowercaseRoutes,
                AppendTrailingSlash = appendTrailingSlash,
            };

            return new AttributeRoute<IController, UrlParameter>(url,
                                      new RouteValueDictionary(),
                                      new RouteValueDictionary(),
                                      new RouteValueDictionary(),
                                      configuration);
        }
    }
}
