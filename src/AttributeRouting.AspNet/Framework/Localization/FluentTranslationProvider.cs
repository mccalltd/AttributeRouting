using System.Web.Routing;
using AttributeRouting.Framework.Localization;

namespace AttributeRouting.Web.Framework.Localization {
    public class FluentTranslationProvider<TController> : FluentTranslationProvider<IRouteConstraint, TController> {
    }
}
