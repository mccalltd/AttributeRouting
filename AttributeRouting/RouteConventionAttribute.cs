using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class RouteConventionAttribute : Attribute
    {
        public abstract IEnumerable<RouteAttribute> GetRouteAttributes(MethodInfo actionMethod);

        public virtual string GetRoutePrefix(MethodInfo action)
        {
            return "";
        }
    }
}
