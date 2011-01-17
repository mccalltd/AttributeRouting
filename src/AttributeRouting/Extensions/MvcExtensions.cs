using System;
using System.Text.RegularExpressions;
using System.Web;

namespace AttributeRouting.Extensions
{
    internal static class MvcExtensions
    {
        public static string GetControllerName(this Type type)
        {
            return Regex.Replace(type.Name, "Controller$", "");
        }

        public static string GetHttpMethod(this HttpRequestBase request)
        {
            return request.Headers["X-HTTP-Method-Override"] ??
                   request.Form["X-HTTP-Method-Override"] ??
                   request.QueryString["X-HTTP-Method-Override"] ??
                   request.HttpMethod;
        }
    }
}