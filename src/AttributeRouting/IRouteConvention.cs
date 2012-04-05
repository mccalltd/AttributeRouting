using System.Collections.Generic;
using System.Reflection;

namespace AttributeRouting {
    public interface IRouteConvention<out TConstraint> {
        /// <summary>
        /// Gets the RouteAttributes to be applied to the given action method.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        IEnumerable<IRouteAttribute> GetRouteAttributes(MethodInfo actionMethod);

        /// <summary>
        /// Gets the default route prefix to use if no RoutePrefix is applied on the controller.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        string GetDefaultRoutePrefix(MethodInfo actionMethod);

        /// <summary>
        /// Gets the route defaults to be applied against the given action method.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        IEnumerable<RouteDefaultAttribute> GetRouteDefaultAttributes(MethodInfo actionMethod);

        /// <summary>
        /// Gets the route constraints to be applied against the given action method.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        IEnumerable<IAttributeRouteConstraint<TConstraint>> GetRouteConstraintAtributes(MethodInfo actionMethod);
    }
}