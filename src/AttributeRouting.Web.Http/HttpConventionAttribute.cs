using System;
using System.Collections.Generic;
using System.Reflection;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http
{
    /// <summary>
    /// Base class for HttpConventionAttributes
    /// </summary>
    public abstract class HttpConventionAttribute : Attribute, IRouteConvention
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

        public virtual IEnumerable<IAttributeRouteConstraint> GetRouteConstraintAttributes(MethodInfo actionMethod)
        {
            yield break;
        }
    }
}
