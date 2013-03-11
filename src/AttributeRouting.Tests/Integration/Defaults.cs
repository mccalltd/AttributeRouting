using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Helpers;
using AttributeRouting.Tests.Subjects;
using AttributeRouting.Tests.Subjects.Mvc;
using AttributeRouting.Web.Constraints;
using AttributeRouting.Web.Mvc;
using NUnit.Framework;

namespace AttributeRouting.Tests.Integration
{
    class BasicUsage
    {
        public BasicUsage()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(cfg =>
            {
                cfg.AddRoutesFromAssemblyOf<MvcApplication>();
                cfg.InlineRouteConstraints.Add("color", typeof(EnumRouteConstraint<Color>));
                cfg.InlineRouteConstraints.Add("colorValue", typeof(EnumValueRouteConstraint<Color>));
            });            
        }

        [Test]
        public void BasicsController()
        {
            GET("/Basics").ShouldMapTo<BasicsController>(c => c.Index());
            POST("/Basics").ShouldMapTo<BasicsController>(c => c.Create());
            PUT("/Basics/1").ShouldMapTo<BasicsController>(c => c.Update("1"));
            DELETE("/Basics/1").ShouldMapTo<BasicsController>(c => c.Delete("1"));
        }

        public RequestAssertionContext GET(string url)
        {
            return new RequestAssertionContext("GET", url);
        }

        public RequestAssertionContext POST(string url)
        {
            return new RequestAssertionContext("POST", url);
        }

        public RequestAssertionContext PUT(string url)
        {
            return new RequestAssertionContext("PUT", url);
        }

        public RequestAssertionContext DELETE(string url)
        {
            return new RequestAssertionContext("DELETE", url);
        }

        public class RequestAssertionContext
        {
            private readonly string _method;
            private readonly string _url;

            public RequestAssertionContext(string method, string url)
            {
                _method = method;
                _url = url;
            }

            public void ShouldMapTo<TController>(Expression<Func<TController, object>> action)
            {
                // Issue a mock request and see what kind of route data we get back.
                var httpContextMock = MockFactory.BuildMockHttpContext(r =>
                {
                    r.Setup(x => x.HttpMethod).Returns(_method);
                    r.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns("~/" + _url.TrimStart('/'));
                    var pathAndQuery = _url.SplitAndTrim("?");
                    if (pathAndQuery.Length > 1)
                    {
                        r.SetupGet(x => x.QueryString).Returns(HttpUtility.ParseQueryString(pathAndQuery[1]));
                    }
                });
                var routeData = RouteTable.Routes.GetRouteData(httpContextMock.Object);
                if (routeData == null)
                {
                    Assert.Fail("No route matched the request {0} {1}.", _method, _url);
                }

                // Do controller, action, and parameters match?
                var methodCallExpression = ((MethodCallExpression)action.Body);
                var expectedControllerName = typeof(TController).GetControllerName();
                var actualControllerName = routeData.Values["controller"];
                Assert.That(expectedControllerName, Is.EqualTo(actualControllerName));
                var expectedActionName = methodCallExpression.Method.Name;
                var actualActionName = routeData.Values["action"];
                Assert.That(expectedActionName, Is.EqualTo(actualActionName));
                var positionalMethodParams = methodCallExpression.Method.GetParameters().OrderBy(p => p.Position).ToArray();
                for (var i = 0; i < methodCallExpression.Arguments.Count; i++)
                {
                    var parameterName = positionalMethodParams[i].Name;
                    var expectedValue = ((ConstantExpression)methodCallExpression.Arguments[i]).Value;
                    var actualValue = routeData.Values[parameterName];
                    Assert.That(expectedValue, Is.EqualTo(actualValue));
                }
            }
        }
    }
}