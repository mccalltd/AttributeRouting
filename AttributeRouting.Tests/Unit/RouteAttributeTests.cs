using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AttributeRouting.Tests.Unit.RouteAttributeTests
{
    public class when_the_get_attribute_is_instantiated
    {
        private readonly GETAttribute _attribute;

        public when_the_get_attribute_is_instantiated()
        {
            _attribute = new GETAttribute("fake");
        }

        [Test]
        public void the_url_is_fake()
        {
            _attribute.Url.ShouldEqual("fake");
        }
        
        [Test]
        public void the_http_method_is_get()
        {
            _attribute.HttpMethod.ShouldEqual("GET");
        }
    }

    public class when_the_delete_attribute_is_instantiated
    {
        private readonly DELETEAttribute _attribute;

        public when_the_delete_attribute_is_instantiated()
        {
            _attribute = new DELETEAttribute("fake");
        }

        [Test]
        public void the_url_is_fake()
        {
            _attribute.Url.ShouldEqual("fake");
        }

        [Test]
        public void the_http_method_is_delete()
        {
            _attribute.HttpMethod.ShouldEqual("DELETE");
        }
    }

    public class when_the_post_attribute_is_instantiated
    {
        private readonly POSTAttribute _attribute;

        public when_the_post_attribute_is_instantiated()
        {
            _attribute = new POSTAttribute("fake");
        }

        [Test]
        public void the_url_is_fake()
        {
            _attribute.Url.ShouldEqual("fake");
        }

        [Test]
        public void the_http_method_is_post()
        {
            _attribute.HttpMethod.ShouldEqual("POST");
        }
    }

    public class when_the_put_attribute_is_instantiated
    {
        private readonly PUTAttribute _attribute;

        public when_the_put_attribute_is_instantiated()
        {
            _attribute = new PUTAttribute("fake");
        }

        [Test]
        public void the_url_is_fake()
        {
            _attribute.Url.ShouldEqual("fake");
        }

        [Test]
        public void the_http_method_is_put()
        {
            _attribute.HttpMethod.ShouldEqual("PUT");
        }
    }
}
