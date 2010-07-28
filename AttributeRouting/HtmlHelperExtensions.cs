using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace AttributeRouting
{
    public static class HtmlHelperExtensions
    {
        public static MvcForm BeginFormPOST(this HtmlHelper helper, string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null)
        {
            return helper.BeginForm(actionName, controllerName, "POST", routeValues, htmlAttributes);
        }

        public static MvcForm BeginFormPUT(this HtmlHelper helper, string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null)
        {
            return helper.BeginForm(actionName, controllerName, "PUT", routeValues, htmlAttributes);
        }

        public static MvcForm BeginFormDELETE(this HtmlHelper helper, string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null)
        {
            return helper.BeginForm(actionName, controllerName, "DELETE", routeValues, htmlAttributes);
        }

        private static MvcForm BeginForm(this HtmlHelper helper, string actionName, string controllerName, string httpMethod, object routeValues = null, object htmlAttributes = null)
        {
            controllerName = controllerName ?? helper.ViewContext.RouteData.GetRequiredString("controller");

            var routeValuesDictionary = new RouteValueDictionary(routeValues);
            routeValuesDictionary.Add("httpMethod", httpMethod);

            var htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);

            var form = helper.BeginForm(actionName, controllerName,
                                        routeValuesDictionary,
                                        FormMethod.Post,
                                        htmlAttributesDictionary);

            if (Regex.IsMatch(httpMethod, "PUT|DELETE", RegexOptions.IgnoreCase))
                helper.ViewContext.HttpContext.Response.Write(helper.HttpMethodOverride(httpMethod));

            return form;
        }
    }
}
