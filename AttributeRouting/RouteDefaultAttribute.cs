using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RouteDefaultAttribute : Attribute
    {
        public RouteDefaultAttribute(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public string ForRouteNamed { get; set; }
    }
}
