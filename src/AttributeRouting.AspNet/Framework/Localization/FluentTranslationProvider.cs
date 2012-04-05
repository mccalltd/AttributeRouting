using System.Web.Routing;
using AttributeRouting.Framework.Localization;

namespace AttributeRouting.AspNet.Framework.Localization {
    public class FluentTranslationProvider<TController> : FluentTranslationProvider<IRouteConstraint, TController> {
    }
}
