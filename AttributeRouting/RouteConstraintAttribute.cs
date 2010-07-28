using System;
using System.Web.Routing;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class RouteConstraintAttribute : Attribute
    {
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
