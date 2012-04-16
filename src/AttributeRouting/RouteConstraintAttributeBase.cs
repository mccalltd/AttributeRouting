using System;
using AttributeRouting.Constraints;

namespace AttributeRouting
{
    /// <summary>
    /// Defines a constraint for a url parameter defined in a RouteAttribute applied to this action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class RouteConstraintAttributeBase : Attribute, IAttributeRouteConstraint
    {
        /// <summary>
        /// Specify a constraint for a url parameter defined in a RouteAttribute applied to this action.
        /// </summary>
        /// <param name="key">The key of the url parameter</param>
        protected RouteConstraintAttributeBase(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            Key = key;
        }

        /// <summary>
        /// The key of the url parameter.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// The constraint to apply against url parameters with the specified key.
        /// </summary>
        public abstract object Constraint { get; }

        /// <summary>
        /// The name of the route to apply this default against.
        /// </summary>
        public string ForRouteNamed { get; set; }
    }
}