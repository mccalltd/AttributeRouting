using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting
{
    public class AttributeRoutingException : Exception
    {
        public AttributeRoutingException(string message)
            : base(message) {}
    }
}
