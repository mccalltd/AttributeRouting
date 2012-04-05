using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Routing;

namespace AttributeRouting.WebApi
{
    /// <summary>
    /// Base class implementors can use to define a custom controller-level route convention.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class RouteConventionAttribute : Attribute, IRouteConvention<IHttpRouteConstraint>
    {
        /// <summary>
        /// Gets the RouteAttributes to be applied to the given action method.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        public abstract IEnumerable<IRouteAttribute> GetRouteAttributes(MethodInfo actionMethod);

        /// <summary>
        /// Gets the default route prefix to use if no RoutePrefix is applied on the controller.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        public virtual string GetDefaultRoutePrefix(MethodInfo actionMethod)
        {
            return "";
        }

        /// <summary>
        /// Gets the route defaults to be applied against the given action method.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        public virtual IEnumerable<RouteDefaultAttribute> GetRouteDefaultAttributes(MethodInfo actionMethod)
        {
            yield break;
        }

        /// <summary>
        /// Gets the route constraints to be applied against the given action method.
        /// </summary>
        /// <param name="actionMethod"></param>
        /// <returns></returns>
        public virtual IEnumerable<IRouteConstraint<IHttpRouteConstraint>> GetRouteConstraintAtributes(MethodInfo actionMethod)
        {
            yield break;
        }
    }
}