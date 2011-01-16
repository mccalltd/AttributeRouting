using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AttributeRouting
{
    internal static class InternalExtensions
    {
        public static string GetControllerName(this Type type)
        {
            return Regex.Replace(type.Name, "Controller$", "");
        }

        public static bool ValueEquals(this string s, string other)
        {
            return s.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasValue(this string s)
        {
            return !String.IsNullOrWhiteSpace(s);
        }

        public static string FormatWith(this string s, params object[] args)
        {
            return String.Format(s, args);
        }

        public static bool IsValidUrl(this string s, bool allowTokens = false)
        {
            var urlParts = s.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            var invalidUrlPattern = @"[#%&\*\\:<>/\+{0}]|\.\.|\.$|^ | $"
                .FormatWith(allowTokens ? "" : @"\{\}?");

            return !urlParts.Any(p => Regex.IsMatch(p, invalidUrlPattern));
        }

        public static string GetHttpMethod(this HttpRequestBase request)
        {
            return request.Headers["X-HTTP-Method-Override"] ??
                   request.Form["X-HTTP-Method-Override"] ??
                   request.QueryString["X-HTTP-Method-Override"] ??
                   request.HttpMethod;
        }

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

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MethodInfo method, bool inherit)
        {
            return method.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>();
        }

        public static IEnumerable<RouteAttribute> GetRouteAttributes(this MethodInfo actionMethod)
        {
            // Yield convention-based attributes
            var conventionalAttributes =
                from attribute in actionMethod.DeclaringType.GetCustomAttributes<RestfulRouteConventionAttribute>(false)
                from routeAttribute in attribute.GetRouteAttributes(actionMethod)
                select routeAttribute;

            foreach (var conventionalAttribute in conventionalAttributes)
                yield return conventionalAttribute;

            // Yield explicitly-defined attributes
            var explicitAttributes =
                from routeAttribute in actionMethod.GetCustomAttributes<RouteAttribute>(false)
                orderby routeAttribute.Order
                select routeAttribute;

            foreach (var explicitAttribute in explicitAttributes)
                yield return explicitAttribute;
        }

        public static RouteAreaAttribute GetRouteAreaAttribute(this MethodInfo method)
        {
            return method.DeclaringType.GetCustomAttributes<RouteAreaAttribute>(true).SingleOrDefault();
        }

        public static RoutePrefixAttribute GetRoutePrefixAttribute(this MethodInfo method)
        {
            return method.DeclaringType.GetCustomAttributes<RoutePrefixAttribute>(true).SingleOrDefault();
        }
    }
}
