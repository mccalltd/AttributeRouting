using System.Collections.Specialized;
using System.Reflection;
using System.Web;

namespace AttributeRouting.Extensions
{
    internal static class HttpRequestBaseExtensions
    {
        public static string GetFormValue(this HttpRequestBase request, string key)
        {
            NameValueCollection collection;

            try
            {
                collection = GetUnvalidatedCollection(request, "Form");
            }
            catch
            {
                collection = request.Form;
            }

            return collection.SafeGet(c => c[key]);
        }

        public static string GetQueryStringValue(this HttpRequestBase request, string key)
        {
            NameValueCollection collection;

            try
            {
                collection = GetUnvalidatedCollection(request, "QueryString");
            }
            catch
            {
                collection = request.QueryString;
            }

            return collection.SafeGet(c => c[key]);
        }

        /// <summary>
        /// Loads the Form or QueryString collection from the unvalidated object in System.Web.Webpages, 
        /// if that assembly is available.
        /// </summary>
        private static NameValueCollection GetUnvalidatedCollection(HttpRequestBase request, string propertyName)
        {
            var webPagesAssembly = Assembly.Load("System.Web.WebPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            var validationType = webPagesAssembly.GetType("System.Web.Helpers.Validation");
            var unvalidatedMethod = validationType.GetMethod("Unvalidated", new[] { request.GetType() });
            var unvalidatedObject = unvalidatedMethod.Invoke(null, new[] { request });
            var collectionProperty = unvalidatedObject.GetType().GetProperty(propertyName);
            var collection = collectionProperty.GetValue(unvalidatedObject, null) as NameValueCollection;

            return collection;
        }
    }
}
