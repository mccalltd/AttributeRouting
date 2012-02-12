using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework.Localization
{
    /// <summary>
    /// Generates conventional keys for use with the translation provder.
    /// </summary>
    public class TranslationKeyGenerator
    {
        public string AreaUrl(string areaName)
        {
            var areaKeyPart = areaName.HasValue() ? areaName + "_" : null;
            return areaKeyPart + "AreaUrl";
        }

        public string AreaUrl<TController>() 
            where TController : IController
        {
            var controllerType = typeof(TController);
            var areaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true);

            if (areaAttribute == null)
                throw new AttributeRoutingException(
                    "There is no RouteAreaAttribute associated with {0}.".FormatWith(controllerType.FullName));

            return AreaUrl(areaAttribute.AreaName);
        }

        public string RoutePrefixUrl(string areaName, string controllerName)
        {
            var areaKeyPart = areaName.HasValue() ? areaName + "_" : null;
            return "{0}{1}_RoutePrefixUrl".FormatWith(areaKeyPart, controllerName);
        }

        public string RoutePrefixUrl<TController>()
            where TController : IController
        {
            var controllerType = typeof(TController);
            var areaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true);

            return RoutePrefixUrl(areaAttribute.SafeGet(a => a.AreaName),
                               controllerType.GetControllerName());
        }

        public string RouteUrl(string areaName, string controllerName, string actionName)
        {
            var area = areaName.HasValue() ? areaName + "_" : null;
            return "{0}{1}_{2}_RouteUrl".FormatWith(area, controllerName, actionName);
        }

        public string RouteUrl<TController>(Expression<Func<TController, object>> action)
            where TController : IController
        {
            var controllerType = typeof(TController);
            var areaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true);
            var actionMemberInfo = Helpers.ExpressionHelper.GetMethodInfo(action);

            return RouteUrl(areaAttribute.SafeGet(a => a.AreaName),
                            controllerType.GetControllerName(),
                            actionMemberInfo.Name);
        }
    }
}
