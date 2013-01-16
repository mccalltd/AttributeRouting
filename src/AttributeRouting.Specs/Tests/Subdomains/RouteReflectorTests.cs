using System.Linq;
using AttributeRouting.Framework;
using AttributeRouting.Specs.Subjects;
using AttributeRouting.Web.Mvc;
using AttributeRouting.Web.Mvc.Framework;
using NUnit.Framework;

namespace AttributeRouting.Specs.Tests.Subdomains
{
    public class RouteReflectorTests
    {
        [Test]
        public void Returns_null_for_area_url_when_subdomain_is_specified_and_area_url_is_not_specified()
        {
            var configuration = new Configuration();
            configuration.AddRoutesFromController<SubdomainController>();

            var reflector = new RouteReflector(configuration);
            var specs = reflector.BuildRouteSpecifications().ToList();

            Assert.That(specs.Count, Is.EqualTo(1));
            Assert.That(specs.Single().Subdomain, Is.EqualTo("users"));
            Assert.That(specs.Single().AreaUrl, Is.Null);
        }

        [Test]
        public void Returns_specified_url_for_area_url_when_both_subdomain_is_specified_and_area_url_is_specified()
        {
            var configuration = new Configuration();
            configuration.AddRoutesFromController<SubdomainWithAreaUrlController>();

            var reflector = new RouteReflector(configuration);
            var specs = reflector.BuildRouteSpecifications().ToList();

            Assert.That(specs.Count, Is.EqualTo(1));
            Assert.That(specs.Single().Subdomain, Is.EqualTo("private"));
            Assert.That(specs.Single().AreaUrl, Is.EqualTo("admin"));
        }

        [Test]
        public void Returns_subdomain_specified_for_area_via_configuration_object()
        {
            var configuration = new Configuration();
            configuration.AddRoutesFromController<SubdomainController>();
            configuration.MapArea("Users").ToSubdomain("override");

            var reflector = new RouteReflector(configuration);
            var specs = reflector.BuildRouteSpecifications().ToList();

            var spec = specs.SingleOrDefault();
            Assert.That(spec, Is.Not.Null);
            Assert.That(spec.Subdomain, Is.EqualTo("override"));
            Assert.That(spec.AreaName, Is.EqualTo("Users"));
            Assert.That(spec.AreaUrl, Is.EqualTo(null));
        }

        [Test]
        public void Returns_null_area_url_when_controller_configured_with_subdomain_only_via_configuration_object()
        {
            var configuration = new Configuration();
            configuration.AddRoutesFromController<SubdomainControllerWithoutSubdomainInAttribute>();
            configuration.MapArea("NoSubdomain").ToSubdomain("subdomain");

            var reflector = new RouteReflector(configuration);
            var specs = reflector.BuildRouteSpecifications().ToList();

            var spec = specs.SingleOrDefault();
            Assert.That(spec, Is.Not.Null);
            Assert.That(spec.Subdomain, Is.EqualTo("subdomain"));
            Assert.That(spec.AreaName, Is.EqualTo("NoSubdomain"));
            Assert.That(spec.AreaUrl, Is.EqualTo(null));
        }
    }
}