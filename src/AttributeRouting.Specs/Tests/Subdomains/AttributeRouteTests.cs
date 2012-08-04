using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Routing;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Mvc;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Subdomains
{
    public class AttributeRouteTests
    {
        [Test]
        public void Route_is_matched_if_subdomain_matches()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());

            const string host = "users.domain.com";
            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.Url).Returns(new Uri("http://" + host, UriKind.Absolute));
                r.SetupGet(x => x.Headers).Returns(new NameValueCollection { { "host", host } });
            });

            var route = routes.Single();
            var data = route.GetRouteData(httpContextMock.Object);

            Assert.That(data, Is.Not.Null);
        }

        [Test]
        public void Route_is_not_matched_if_subdomain_does_not_match()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());

            const string host = "www.domain.com";
            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.Url).Returns(new Uri("http://" + host, UriKind.Absolute));
                r.SetupGet(x => x.Headers).Returns(new NameValueCollection { { "host", host } });
            });

            var route = routes.Single();
            var data = route.GetRouteData(httpContextMock.Object);

            Assert.That(data, Is.Null);
        }

        [Test]
        public void Route_is_not_matched_if_subdomain_is_not_mapped_and_is_not_equal_to_configured_default()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());

            const string host = "whatever.domain.com";
            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.Url).Returns(new Uri("http://" + host, UriKind.Absolute));
                r.SetupGet(x => x.Headers).Returns(new NameValueCollection { { "host", host } });
            });

            var route = routes.Single();
            var data = route.GetRouteData(httpContextMock.Object);

            Assert.That(data, Is.Null);
        }

        [Test]
        public void Route_is_matched_if_subdomain_is_not_mapped_and_parsed_subdomain_is_null()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
            });

            const string host = "localhost";
            var httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.Url).Returns(new Uri("http://" + host, UriKind.Absolute));
                r.SetupGet(x => x.Headers).Returns(new NameValueCollection { { "host", host } });
            });

            var route = routes.First();
            var data = route.GetRouteData(httpContextMock.Object);

            Assert.That(data, Is.Not.Null);
        }
    }
}