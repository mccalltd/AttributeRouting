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

        public string RoutePrefix(string areaName, string controllerName)
        {
            var areaKeyPart = areaName.HasValue() ? areaName + "_" : null;
            return "{0}{1}_RoutePrefix".FormatWith(areaKeyPart, controllerName);
        }

        public string RouteUrl(string areaName, string controllerName, string actionName)
        {
            var area = areaName.HasValue() ? areaName + "_" : null;
            return "{0}{1}_{2}_RouteUrl".FormatWith(area, controllerName, actionName);
        }
    }
}
