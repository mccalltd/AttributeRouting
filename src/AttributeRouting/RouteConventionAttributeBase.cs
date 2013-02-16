using System;
using System.Collections.Generic;
using System.Reflection;

namespace AttributeRouting
{
    /// <summary>
    /// Base class that implementors can use to define a custom controller-level route convention.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class RouteConventionAttributeBase : Attribute
    {
        /// <summary>
        /// Gets the RouteAttributes to be applied to the given action method.
        /// </summary>
        /// <param name="actionMethod">The action method.</param>
        /// <returns>A collection of routes for the action.</returns>
        public abstract IEnumerable<IRouteAttribute> GetRouteAttributes(MethodInfo actionMethod);

        /// <summary>
        /// Gets the default route prefix to use if no RoutePrefix is applied on the controller.
        /// </summary>
        /// <param name="actionMethod">The action method.</param>
        /// <returns>A default prefix to use for the actions in the controller.</returns>
        [Obsolete("Use GetDefaultRoutePrefixes instead.")]
        public virtual string GetDefaultRoutePrefix(MethodInfo actionMethod)
        {
            return "";
        }

        /// <summary>
        /// Gets a <see cref="RoutePrefixAttribute"/> to apply to the controller 
        /// if no explicit route prefix is specified.
        /// </summary>
        /// <param name="controllerType">The controller type.</param>
        /// <returns>A <see cref="RoutePrefixAttribute"/>.</returns>
        public virtual IEnumerable<RoutePrefixAttribute> GetDefaultRoutePrefixes(Type controllerType)
        {
            yield break;
        }

        /// <summary>
        /// Gets a <see cref="RouteAreaAttribute"/> to apply to the controller 
        /// if no explicit route area is specified.
        /// </summary>
        /// <param name="controllerType">The controller type.</param>
        /// <returns>A <see cref="RoutePrefixAttribute"/>.</returns>
        public virtual RouteAreaAttribute GetDefaultRouteArea(Type controllerType)
        {
            return null;
        }
    }
}