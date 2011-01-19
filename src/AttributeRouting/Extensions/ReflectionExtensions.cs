using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace AttributeRouting.Extensions
{
    internal static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetControllerTypes(this Assembly assembly)
        {
            return from type in assembly.GetTypes()
                   where !type.IsAbstract && typeof(Controller).IsAssignableFrom(type)
                   select type;
        }

        public static IEnumerable<MethodInfo> GetActionMethods(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            return from method in type.GetMethods(bindingFlags)
                   where typeof(ActionResult).IsAssignableFrom(method.ReturnType)
                   select method;
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