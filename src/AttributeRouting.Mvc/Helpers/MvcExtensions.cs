using System;
using System.Text.RegularExpressions;
using System.Web;
using AttributeRouting.Helpers;

namespace AttributeRouting.Mvc.Helpers
{
    internal static class MvcExtensions
    {
        public static string GetHttpMethod(this HttpRequestBase request)
        {
            return ObjectExtensions.SafeGet(request, r => r.Headers["X-HTTP-Method-Override"]) ??
                   ObjectExtensions.SafeGet(request, r => HttpRequestBaseExtensions.GetFormValue(r, "X-HTTP-Method-Override")) ??
                   ObjectExtensions.SafeGet(request, r => HttpRequestBaseExtensions.GetQueryStringValue(r, "X-HTTP-Method-Override")) ??
                   ObjectExtensions.SafeGet(request, r => r.HttpMethod, "GET");
        }
    }
}