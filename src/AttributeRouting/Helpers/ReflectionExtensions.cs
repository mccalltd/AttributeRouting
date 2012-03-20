using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace AttributeRouting.Helpers
{
    internal static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetControllerTypes(this Assembly assembly)
        {
            return from type in assembly.GetTypes()
                   where !type.IsAbstract && typeof(IController).IsAssignableFrom(type)
                   select type;
        }

        public static IEnumerable<MethodInfo> GetActionMethods(this Type type, bool inheritActionsFromBaseController)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;

            if (!inheritActionsFromBaseController)
                flags |= BindingFlags.DeclaredOnly;

            return type.GetMethods(flags);
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this Type type, bool inherit)
        {
            return type.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>();
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this Type type, bool inherit)
        {
            return type.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().FirstOrDefault();
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MethodInfo method, bool inherit)
        {
            return method.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>();
        }
    }
}