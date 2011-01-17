using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RouteDefaultAttribute : Attribute
    {
        /// <summary>
        /// Specify a default value for a url parameter defined in a RouteAttribute applied to this action.
        /// </summary>
        /// <param name="key">The key of the url parameter</param>
        /// <param name="value">The default value for the url parameter</param>
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
