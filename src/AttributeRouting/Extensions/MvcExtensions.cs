using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;

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
                   request.Unvalidated().Form["X-HTTP-Method-Override"] ??
                   request.Unvalidated().QueryString["X-HTTP-Method-Override"] ??
                   request.HttpMethod;
        }
    }
}