using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Helpers;

namespace AttributeRouting.Mvc.Framework.Localization {
    public class MvcTranslationKeyGenerator : TranslationKeyGenerator  {
        /// <summary>
        /// Generates the conventional key for the url of the area on the specified controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller</typeparam>
        public string AreaUrl<TController>()
            where TController : IController {
            var controllerType = typeof(TController);

            var areaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true);
            if (areaAttribute == null)
                throw new AttributeRoutingException(
                    "There is no RouteAreaAttribute associated with {0}.".FormatWith(controllerType.FullName));

            return AreaUrl(areaAttribute.AreaName);
        }

        /// <summary>
        /// Generates the conventional key for the url of the route prefix on the specified controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller</typeparam>
        public string RoutePrefixUrl<TController>()
            where TController : IController {
            var controllerType = typeof(TController);

            var routePrefixAttribute = controllerType.GetCustomAttribute<RoutePrefixAttribute>(true);
            if (routePrefixAttribute == null)
                throw new AttributeRoutingException(
                    "There is no RoutePrefixAttribute associated with {0}.".FormatWith(controllerType.FullName));

            var areaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true);

            return RoutePrefixUrl(areaAttribute.SafeGet(a => a.AreaName), controllerType.GetControllerName());
        }

        /// <summary>
        /// Generates the conventional key for the url of the route on the specified action method.
        /// </summary>
        /// <typeparam name="TController">The type of the controller</typeparam>
        /// <param name="action">Expression pointing to the the action method</param>
        public string RouteUrl<TController>(Expression<Func<TController, object>> action)
            where TController : IController {
            var controllerType = typeof(TController);
            var areaAttribute = controllerType.GetCustomAttribute<RouteAreaAttribute>(true);
            var actionMemberInfo = Helpers.ExpressionHelper.GetMethodInfo(action);

            return RouteUrl(areaAttribute.SafeGet(a => a.AreaName),
                            controllerType.GetControllerName(),
                            actionMemberInfo.Name);
        }
    }
}
