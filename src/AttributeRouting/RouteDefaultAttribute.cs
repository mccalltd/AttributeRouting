using System;

namespace AttributeRouting
{
    /// <summary>
    /// Defines a default value for a url parameter defined in a RouteAttribute applied to this action.
    /// </summary>
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

        /// <summary>
        /// The key of the url parameter.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The default value for the url parameter.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The name of the route to apply this default against.
        /// </summary>
        public string ForRouteNamed { get; set; }
    }
}