using System.Linq;
using System.Web.Routing;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Mvc;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.LowercaseUrls
{
    public class AttributeRouteTests
    {
        [Test]
        public void It_returns_routes_with_everything_lowered_including_params_when_configured_globally()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c =>
            {
                c.AddRoutesFromController<LowercaseUrlController>();
                c.UseLowercaseRoutes = true;
                c.PreserveCaseForUrlParameters = false;
            });

            var route = routes.Cast<Route>().FirstOrDefault();

            Assert.That(route, Is.Not.Null);

            var requestContext = MockBuilder.BuildRequestContext();

            var pathData = route.GetVirtualPath(requestContext, new RouteValueDictionary
            {
                { "userName", "CharlieChan" },
                { "queryString", "WhatTimeIsIt" }
            });

            Assert.That(pathData, Is.Not.Null);
            Assert.That(pathData.VirtualPath, Is.EqualTo("lowercaseurl/hello/charliechan/goodbye?queryString=WhatTimeIsIt"));
        }

        [Test]
        public void It_returns_routes_with_everything_lowered_including_params_when_configured_via_route_attribute()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c => c.AddRoutesFromController<LowercaseUrlController>());

            var route = routes.Cast<Route>().ElementAt(1);

            Assert.That(route, Is.Not.Null);

            var requestContext = MockBuilder.BuildRequestContext();

            var pathData = route.GetVirtualPath(requestContext, new RouteValueDictionary
            {
                { "routeParam", "CharlieChan" },
                { "queryString", "WhatTimeIsIt" }
            });

            Assert.That(pathData, Is.Not.Null);
            Assert.That(pathData.VirtualPath, Is.EqualTo("lowercaseurl/lowercase-override/charliechan?queryString=WhatTimeIsIt"));
        }

        [Test]
        public void It_returns_routes_with_everything_lowered_except_params_and_query_strings_when_configured_globally()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c =>
            {
                c.AddRoutesFromController<LowercaseUrlController>();
                c.UseLowercaseRoutes = true;
                c.PreserveCaseForUrlParameters = true;
            });

            var route = routes.Cast<Route>().FirstOrDefault();

            Assert.That(route, Is.Not.Null);

            var requestContext = MockBuilder.BuildRequestContext();

            var pathData = route.GetVirtualPath(requestContext, new RouteValueDictionary
            {
                { "userName", "CharlieChan" },
                { "queryString", "WhatTimeIsIt" }
            });

            Assert.That(pathData, Is.Not.Null);
            Assert.That(pathData.VirtualPath, Is.EqualTo("lowercaseurl/hello/CharlieChan/goodbye?queryString=WhatTimeIsIt"));
        }

        [Test]
        public void It_returns_routes_with_everything_lowered_except_params_and_query_strings_when_configured_via_route_attribute()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c => c.AddRoutesFromController<LowercaseUrlController>());

            var route = routes.Cast<Route>().ElementAt(2);

            Assert.That(route, Is.Not.Null);

            var requestContext = MockBuilder.BuildRequestContext();

            var pathData = route.GetVirtualPath(requestContext, new RouteValueDictionary
            {
                { "routeParam", "CharlieChan" },
                { "queryString", "WhatTimeIsIt" }
            });

            Assert.That(pathData, Is.Not.Null);
            Assert.That(pathData.VirtualPath, Is.EqualTo("lowercaseurl/lowercase-preserve-url-param-case-override/CharlieChan?queryString=WhatTimeIsIt"));
        }
    }
}