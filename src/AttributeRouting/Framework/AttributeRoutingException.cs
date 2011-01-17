using System;

namespace AttributeRouting.Framework
{
    public class AttributeRoutingException : Exception
    {
        public AttributeRoutingException(string message)
            : base(message) {}
    }
}