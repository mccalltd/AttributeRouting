using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Mvc;
using AttributeRouting.Web.Mvc.Extensions;
using Moq;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Extensions
{
    public class UrlHelperExtensionTests
    {
        private UrlHelper GetUrlHelper(RouteCollection routes, string host, string schema = "http")
        {
            Mock<HttpContextBase> httpContextMock = MockBuilder.BuildMockHttpContext(r =>
            {
                r.SetupGet(x => x.Url).Returns(new Uri(schema + "://" + host, UriKind.Absolute));
                r.SetupGet(x => x.Headers).Returns(new NameValueCollection { { "host", host } });
            });

            return new UrlHelper(new RequestContext(httpContextMock.Object, new RouteData()), routes);
        }

        #region Action

        [Test]
        public void Local_Host_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());
            var helper = GetUrlHelper(routes, "localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", "Users");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.localhost/", path);
        }

        [Test]
        public void Top_Level_Domain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());
            var helper = GetUrlHelper(routes, "example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", "Users");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.com/", path);
        }

        [Test]
        public void Second_Level_Domain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());
            var helper = GetUrlHelper(routes, "example.co.uk");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", "Users");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.co.uk/", path);
        }

        [Test]
        public void Top_Level_Domain_With_Subdomain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainWithAreaUrlController>();
                config.AddRoutesFromController<SubdomainController>();
            });
            var helper = GetUrlHelper(routes, "private.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", "Users");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.com/", path);
        }

        [Test]
        public void Second_Level_Domain_With_Subdomain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainWithAreaUrlController>();
                config.AddRoutesFromController<SubdomainController>();
            });
            var helper = GetUrlHelper(routes, "private.example.co.ok");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", "Users");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.co.ok/", path);
        }

        [Test]
        public void Current_Subdomain_Action_To_Same_Subdomain_Will_Only_Be_The_Relative_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", "Users");
            Console.WriteLine(path);
            Assert.AreEqual("/", path);
        }

        [Test]
        public void Current_Subdomain_Action_To_No_Subdomain_And_Default_Subdomain_Will_Return_Full_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
            });
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("AnyVerb", "StandardUsage");
            Console.WriteLine(path);
            Assert.AreEqual("http://www.example.com/AnyVerb", path);
        }

        [Test]
        public void Current_Subdomain_Action_To_No_Subdomain_And_Custom_Default_Subdomain_Will_Return_Full_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
                config.DefaultSubdomain = "xyz";
            });
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("AnyVerb", "StandardUsage");
            Console.WriteLine(path);
            Assert.AreEqual("http://xyz.example.com/AnyVerb", path);
        }

        [Test]
        public void Current_Subdomain_Action_To_No_Subdomain_And_No_Default_Subdomain_Will_Return_Full_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
                config.DefaultSubdomain = "";
            });
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("AnyVerb", "StandardUsage");
            Console.WriteLine(path);
            Assert.AreEqual("http://example.com/AnyVerb", path);
        }

        [Test]
        public void Local_Host_With_Subdomain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainWithAreaUrlController>();
                config.AddRoutesFromController<SubdomainController>();
            });
            var helper = GetUrlHelper(routes, "private.localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", "Users");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.localhost/", path);
        }


        [Test]
        public void Local_Host_With_Subdomain_Action_To_Non_Area_Method_Will_Return_Full_Url_Without_Default_Subdomain()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
            });
            var helper = GetUrlHelper(routes, "users.localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "StandardUsage");
            Console.WriteLine(path);
            Assert.AreEqual("http://localhost/", path);
        }


        [Test]
        public void HTTP_URL_Will_Change_To_HTTPS()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());
            var helper = GetUrlHelper(routes, "localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", new {area = "Users"}, "https");
            Console.WriteLine(path);
            Assert.AreEqual("https://users.localhost/", path);
        }

        [Test]
        public void HTTPS_URL_Will_Change_To_HTTP()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());
            var helper = GetUrlHelper(routes, "localhost", "https");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", new {area = "Users"}, "http");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.localhost/", path);
        }

        [Test]
        public void Host_With_Port_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config => config.AddRoutesFromController<SubdomainController>());
            var helper = GetUrlHelper(routes, "example.com:81");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainAction("Index", "Subdomain", new {area = "Users"});
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.com:81/", path);
        }

        #endregion

        #region RouteURL

        [Test]
        public void RouteURL_Local_Host_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.localhost/", path);
        }


        [Test]
        public void RouteURL_Top_Level_Domain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.com/", path);
        }

        [Test]
        public void RouteURL_Second_Level_Domain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "example.co.uk");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.co.uk/", path);
        }

        [Test]
        public void RouteURL_Top_Level_Domain_With_Subdomain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainWithAreaUrlController>();
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "private.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.com/", path);
        }

        [Test]
        public void RouteURL_Second_Level_Domain_With_Subdomain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainWithAreaUrlController>();
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "private.example.co.ok");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.co.ok/", path);
        }

        [Test]
        public void RouteURL_Current_Subdomain_Action_To_Same_Subdomain_Will_Only_Be_The_Relative_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("/", path);
        }

        [Test]
        public void RouteURL_Current_Subdomain_Action_To_No_Subdomain_And_Default_Subdomain_Will_Return_Full_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("StandardUsage_AnyVerb");
            Console.WriteLine(path);
            Assert.AreEqual("http://www.example.com/AnyVerb", path);
        }

        [Test]
        public void RouteURL_Current_Subdomain_Action_To_No_Subdomain_And_Custom_Default_Subdomain_Will_Return_Full_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
                config.DefaultSubdomain = "xyz";
            });
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("StandardUsage_AnyVerb");
            Console.WriteLine(path);
            Assert.AreEqual("http://xyz.example.com/AnyVerb", path);
        }

        [Test]
        public void RouteURL_Current_Subdomain_Action_To_No_Subdomain_And_No_Default_Subdomain_Will_Return_Full_Url()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
                config.DefaultSubdomain = "";
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "users.example.com");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("StandardUsage_AnyVerb");
            Console.WriteLine(path);
            Assert.AreEqual("http://example.com/AnyVerb", path);
        }

        [Test]
        public void RouteURL_Local_Host_With_Subdomain_Action_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainWithAreaUrlController>();
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "private.localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.localhost/", path);
        }


        [Test]
        public void
            RouteURL_Local_Host_With_Subdomain_Action_To_Non_Area_Method_Will_Return_Full_Url_Without_Default_Subdomain()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<StandardUsageController>();
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "users.localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("StandardUsage_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://localhost/", path);
        }


        [Test]
        public void RouteURL_HTTP_URL_Will_Change_To_HTTPS()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "localhost");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index", null, "https");
            Console.WriteLine(path);
            Assert.AreEqual("https://users.localhost/", path);
        }

        [Test]
        public void RouteURL_HTTPS_URL_Will_Change_To_HTTP()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "localhost", "https");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index", null, "http");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.localhost/", path);
        }

        [Test]
        public void RouteURL_Host_With_Port_Will_Have_A_Subdomain_Prepended()
        {
            var routes = RouteTable.Routes;
            routes.Clear();
            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromController<SubdomainController>();
                config.AutoGenerateRouteNames = true;
            });
            var helper = GetUrlHelper(routes, "example.com:81");
            Assert.That(helper, Is.Not.Null);
            var path = helper.SubdomainRouteUrl("Users_Subdomain_Index");
            Console.WriteLine(path);
            Assert.AreEqual("http://users.example.com:81/", path);
        }

        #endregion

    }
}