using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Specs.Subjects;
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
        public void It_does_not_append_trailing_slash_to_urls_when_not_configured() 
        {
            var route = BuildAttributeRoute("Controller/Action", false, false);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(route.Url));
        }

        [Test]
        public void It_returns_paths_with_trailing_slash_when_configured_gloablly() 
        {
            var route = BuildAttributeRoute("Controller/Action", false, true);

            var routeValues = new RouteValueDictionary(new { query = "MixedCase" });
            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, routeValues);

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath,
                        Is.EqualTo(route.Url + "/?query=MixedCase"));
        }

        [Test]
        public void It_returns_paths_with_trailing_slash_when_configured_via_route_attribute() 
        {
            // Arrange
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c =>
            {
                c.AddRoutesFromController<TrailingSlashesController>();
                c.UseLowercaseRoutes = true;
            });

            var route = routes.Cast<Route>().ElementAt(1);
            Assert.That(route, Is.Not.Null);

            // Act
            var requestContext = MockBuilder.BuildRequestContext();
            var pathData = route.GetVirtualPath(requestContext, new RouteValueDictionary
            {
                { "queryString", "WhatTimeIsIt" }
            });

            // Assert
            Assert.That(pathData, Is.Not.Null);
            Assert.That(pathData.VirtualPath, Is.EqualTo("trailing-slash/route-override-true/?queryString=WhatTimeIsIt"));
        }

        [Test]
        public void It_does_not_return_paths_with_trailing_slash_when_configured_via_route_attribute() 
        {
            // Arrange
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c =>
            {
                c.AddRoutesFromController<TrailingSlashesController>();
                c.UseLowercaseRoutes = true;
                c.AppendTrailingSlash = true;
            });

            var route = routes.Cast<Route>().ElementAt(2);
            Assert.That(route, Is.Not.Null);

            // Act
            var requestContext = MockBuilder.BuildRequestContext();
            var pathData = route.GetVirtualPath(requestContext, new RouteValueDictionary
            {
                { "queryString", "WhatTimeIsIt" }
            });

            // Assert
            Assert.That(pathData, Is.Not.Null);
            Assert.That(pathData.VirtualPath, Is.EqualTo("trailing-slash/route-override-false?queryString=WhatTimeIsIt"));
        }

        [Test]
        public void It_does_not_append_trailing_slash_to_root_urls_when_configured_for_trailing_slashes() 
        {
            var route = BuildAttributeRoute("", false, true);

            var virtualPathData = route.GetVirtualPath(_requestContextMock.Object, new RouteValueDictionary());

            Assert.That(virtualPathData, Is.Not.Null);
            Assert.That(virtualPathData.VirtualPath, Is.EqualTo(""));
        }

        private Route BuildAttributeRoute(string url, bool useLowercaseRoutes, bool appendTrailingSlash)
        {
            var configuration = new Configuration
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
