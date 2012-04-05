using System;
using System.Linq.Expressions;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework.Localization
{
    /// <summary>
    /// Generates conventional keys for use with the translation provder.
    /// </summary>
    public class TranslationKeyGenerator
    {
        /// <summary>
        /// Generates the conventional key for the url of the specified area.
        /// </summary>
        /// <param name="areaName">The name of the area</param>
        public string AreaUrl(string areaName)
        {
            var areaKeyPart = areaName.HasValue() ? areaName + "_" : null;
            return areaKeyPart + "AreaUrl";
        }        

        /// <summary>
        /// Generates the conventional key for the url of the specified route prefix.
        /// </summary>
        /// <param name="areaName">The name of the area, if applicable</param>
        /// <param name="controllerName">The name of the controller</param>
        public string RoutePrefixUrl(string areaName, string controllerName)
        {
            var areaKeyPart = areaName.HasValue() ? areaName + "_" : null;
            return "{0}{1}_RoutePrefixUrl".FormatWith(areaKeyPart, controllerName);
        }        

        /// <summary>
        /// Generates the conventional key for the url of the specified route.
        /// </summary>
        /// <param name="areaName">The name of the area, if applicable</param>
        /// <param name="controllerName">The name of the controller</param>
        /// <param name="actionName">The name of the action method</param>
        public string RouteUrl(string areaName, string controllerName, string actionName)
        {
            var area = areaName.HasValue() ? areaName + "_" : null;
            return "{0}{1}_{2}_RouteUrl".FormatWith(area, controllerName, actionName);
        }        
    }
}
