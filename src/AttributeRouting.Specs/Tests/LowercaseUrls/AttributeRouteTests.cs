using System.Linq;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Specs.Subjects;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.LowercaseUrls
{
    public class AttributeRouteTests
    {
        [Test]
        public void It_returns_routes_with_everything_lowered_except_params_and_query_strings_when_so_configured()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c =>
            {
                c.AddRoutesFromController<LowercaseUrlController>();
                c.UseLowercaseRoutes = true;
                c.PreserveCaseForUrlParameters = true;
            });

            var route = routes.Cast<AttributeRoute>().FirstOrDefault();

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
        public void It_returns_routes_with_everything_lowered_including_params_and_query_strings_when_so_configured()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(c =>
            {
                c.AddRoutesFromController<LowercaseUrlController>();
                c.UseLowercaseRoutes = true;
                c.PreserveCaseForUrlParameters = false;
            });

            var route = routes.Cast<AttributeRoute>().FirstOrDefault();

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
    }
}