using System.Web.Mvc;
using AttributeRouting.Framework;

namespace AttributeRouting.Mvc.Framework {
    internal class UrlParameterFactory : IParameterFactory<UrlParameter> {
        /// <summary>
        /// Optional parameter
        /// </summary>
        /// <typeparam name="TRouteParameter"></typeparam>
        /// <returns></returns>
        public UrlParameter Optional() {
            return UrlParameter.Optional;
        }
    }
}