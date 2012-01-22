using System.Collections.Specialized;
using System.Reflection;
using System.Web;

namespace AttributeRouting.Extensions
{
    internal static class HttpRequestBaseExtensions
    {
        private static bool _isSystemWebWebPagesUnavailable;

        public static string GetFormValue(this HttpRequestBase request, string key)
        {
            return request.GetUnvalidatedCollectionOr("Form", request.Form)[key];
        }

        public static string GetQueryStringValue(this HttpRequestBase request, string key)
        {
            return request.GetUnvalidatedCollectionOr("QueryString", request.QueryString)[key];
        }

        /// <summary>
        /// Loads the Form or QueryString collection from the unvalidated object in System.Web.Webpages, 
        /// if that assembly is available.
        /// </summary>
        private static NameValueCollection GetUnvalidatedCollectionOr(this HttpRequestBase request, string unvalidatedObjectPropertyName, NameValueCollection defaultCollection)
        {
            if (_isSystemWebWebPagesUnavailable)
                return defaultCollection;

            try
            {
                var webPagesAssembly = Assembly.Load("System.Web.WebPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                var validationType = webPagesAssembly.GetType("System.Web.Helpers.Validation");
                var unvalidatedMethod = validationType.GetMethod("Unvalidated", new[] { request.GetType() });
                var unvalidatedObject = unvalidatedMethod.Invoke(null, new[] { request });
                var collectionProperty = unvalidatedObject.GetType().GetProperty(unvalidatedObjectPropertyName);
                var collection = collectionProperty.GetValue(unvalidatedObject, null) as NameValueCollection;

                return collection;
            }
            catch
            {
                _isSystemWebWebPagesUnavailable = true;

                return defaultCollection;
            }
        }
    }
}
