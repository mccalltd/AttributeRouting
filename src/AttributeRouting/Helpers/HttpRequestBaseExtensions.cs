using System.Collections.Specialized;
using System.Reflection;
using System.Web;

namespace AttributeRouting.Helpers
{
    internal static class HttpRequestBaseExtensions
    {
        private static bool _isSystemWebWebPagesUnavailable;

        public static string GetFormValue(this HttpRequestBase request, string key)
        {
            return request.GetUnvalidatedCollectionValue("Form", key) ?? request.Form[key];
        }

        public static string GetQueryStringValue(this HttpRequestBase request, string key)
        {
            return request.GetUnvalidatedCollectionValue("QueryString", key) ?? request.QueryString[key];
        }

        /// <summary>
        /// Loads the Form or QueryString collection value from the unvalidated object in System.Web.Webpages, 
        /// if that assembly is available.
        /// </summary>
        private static string GetUnvalidatedCollectionValue(this HttpRequestBase request, string unvalidatedObjectPropertyName, string key)
        {
            if (_isSystemWebWebPagesUnavailable)
                return null;

            try
            {
                var webPagesAssembly = Assembly.Load("System.Web.WebPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                var validationType = webPagesAssembly.GetType("System.Web.Helpers.Validation");
                var unvalidatedMethod = validationType.GetMethod("Unvalidated", new[] { request.GetType() });
                var unvalidatedObject = unvalidatedMethod.Invoke(null, new[] { request });
                var collectionProperty = unvalidatedObject.GetType().GetProperty(unvalidatedObjectPropertyName);
                var collection = (NameValueCollection)collectionProperty.GetValue(unvalidatedObject, null);

                return collection[key];
            }
            catch
            {
                _isSystemWebWebPagesUnavailable = true;

                return null;
            }
        }
    }
}
