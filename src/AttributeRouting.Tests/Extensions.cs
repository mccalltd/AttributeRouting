using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AttributeRouting.Tests
{
    public static class Extensions
    {
        public static void ShouldEqual(this object obj, object expected)
        {
            Assert.That(obj, Is.EqualTo(expected));
        }

        public static void ShouldBeFalse(this object obj)
        {
            Assert.That(obj, Is.False);
        }

        public static void ShouldBeTrue(this object obj)
        {
            Assert.That(obj, Is.True);
        }

        public static void ShouldNotBeNull(this object obj)
        {
            Assert.That(obj, Is.Not.Null);
        }
    }
}
