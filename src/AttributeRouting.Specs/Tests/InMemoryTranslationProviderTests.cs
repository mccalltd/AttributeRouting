using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Specs.Subjects;
using Moq;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests
{
    public class InMemoryTranslationProviderTests
    {
        private RequestContext _requestContext;

        [SetUp]
        public void SetUp()
        {
            var requestMock = new Mock<HttpRequestBase>(MockBehavior.Strict);
            requestMock.SetupGet(x => x.ApplicationPath).Returns("/");
            requestMock.SetupGet(x => x.Url).Returns(new Uri("http://localhost/", UriKind.Absolute));
            requestMock.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());

            var responseMock = new Mock<HttpResponseBase>(MockBehavior.Strict);
            responseMock.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(url => url);

            var httpContextMock = new Mock<HttpContextBase>(MockBehavior.Strict);
            httpContextMock.SetupGet(x => x.Request).Returns(requestMock.Object);
            httpContextMock.SetupGet(x => x.Response).Returns(responseMock.Object);

            _requestContext = new RequestContext(httpContextMock.Object, new RouteData());
        }
        
        [Test]
        public void Usage()
        {
            /**
             * TranslationProvider usage
             */

            var translations = new InMemoryTranslationProvider();
                
            translations.Configure()
                .ByKey("Area_AreaUrl", new Dictionary<string, string>
                {
                    { "es", "es-Area" }
                })
                .ByKey("Area_Translation_RoutePrefix", new Dictionary<string, string>
                {
                    { "es", "es-Prefix" }
                })
                .ByKey("Area_Translation_Index_RouteUrl", new Dictionary<string, string>
                {
                    { "es", "es-Index" }
                });

            Assert.That(translations.Translate("Area_AreaUrl", "en"), Is.Null);
            Assert.That(translations.Translate("Area_Translation_RoutePrefix", "en"), Is.Null);
            Assert.That(translations.Translate("Area_Translation_Index_RouteUrl", "en"), Is.Null);

            Assert.That(translations.Translate("Area_AreaUrl", "es"), Is.EqualTo("es-Area"));
            Assert.That(translations.Translate("Area_Translation_RoutePrefix", "es"), Is.EqualTo("es-Prefix"));
            Assert.That(translations.Translate("Area_Translation_Index_RouteUrl", "es"), Is.EqualTo("es-Index"));
            
            Assert.That(translations.CultureNames.Count(), Is.EqualTo(1));
            Assert.That(translations.CultureNames.First(), Is.EqualTo("es"));

            /**
             * TranslationProvider configuration
             */

            RouteTable.Routes.Clear();
            RouteTable.Routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<TranslationController>();
                config.TranslationProvider = translations;
            });

            /**
             * UrlHelper usage
             */

            // Default culture
            var urlHelper = new UrlHelper(_requestContext, RouteTable.Routes);
            Assert.That(urlHelper.Action("Index", "Translation", new { area = "Area" }),
                        Is.EqualTo("/Area/Prefix/Index"));
            
            // es-ES culture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
            Assert.That(urlHelper.Action("Index", "Translation", new { area = "Area" }),
                        Is.EqualTo("/es-Area/es-Prefix/es-Index"));

            // es culture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
            Assert.That(urlHelper.Action("Index", "Translation", new { area = "Area" }),
                        Is.EqualTo("/es-Area/es-Prefix/es-Index"));
        }
    }
}
