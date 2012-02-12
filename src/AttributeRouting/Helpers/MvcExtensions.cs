using System;
using System.Text.RegularExpressions;
using System.Web;

namespace AttributeRouting.Helpers
{
    internal static class MvcExtensions
    {
        public static string GetControllerName(this Type type)
        {
            return Regex.Replace(type.Name, "Controller$", "");
        }

        public static string GetHttpMethod(this HttpRequestBase request)
        {
            return request.SafeGet(r => r.Headers["X-HTTP-Method-Override"]) ??
                   request.SafeGet(r => r.GetFormValue("X-HTTP-Method-Override")) ??
                   request.SafeGet(r => r.GetQueryStringValue("X-HTTP-Method-Override")) ??
                   request.SafeGet(r => r.HttpMethod, "GET");
        }
    }
}