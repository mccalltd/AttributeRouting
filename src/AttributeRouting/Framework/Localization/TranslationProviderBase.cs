using System.Collections.Generic;
using AttributeRouting.Extensions;

namespace AttributeRouting.Framework.Localization
{
    public abstract class TranslationProviderBase
    {
        public abstract IEnumerable<string> CultureNames { get; }

        public abstract string Translate(string key, string culture);

        public string GetRouteUrl(string cultureName, RouteSpecification routeSpec)
        {
            var area = (routeSpec.AreaName.HasValue()) ? routeSpec.AreaName + "_" : null;
            var key = "{0}{1}_{2}_RouteUrl".FormatWith(area, routeSpec.ControllerName, routeSpec.ActionName);

            return Translate(key, cultureName);
        }

        public string GetRoutePrefix(string cultureName, RouteSpecification routeSpec)
        {
            var area = (routeSpec.AreaName.HasValue()) ? routeSpec.AreaName + "_" : null;
            var key = "{0}{1}_RoutePrefix".FormatWith(area, routeSpec.ControllerName);

            return Translate(key, cultureName);
        }

        public string GetAreaUrl(string cultureName, RouteSpecification routeSpec)
        {
            var area = (routeSpec.AreaName.HasValue()) ? routeSpec.AreaName + "_" : null;
            var key = "{0}AreaUrl".FormatWith(area, routeSpec.ControllerName);

            return Translate(key, cultureName);
        }
    }
}