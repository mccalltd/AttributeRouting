using System.Web.Mvc;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Mvc.Framework.Factories {
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