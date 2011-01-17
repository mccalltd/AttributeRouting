using System;
using System.Web.Routing;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class RouteConstraintAttribute : Attribute
    {
        /// <summary>
        /// Specify a constraint for a url parameter defined in a RouteAttribute applied to this action.
        /// </summary>
        /// <param name="key">The key of the url parameter</param>
        protected RouteConstraintAttribute(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            Key = key;
        }

        public string Key { get; private set; }

        public string ForRouteNamed { get; set; }

        public abstract IRouteConstraint Constraint { get; }
    }
}
