using System.Web.Http;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Mvc.Framework.Factories {
    internal class UrlParameterFactory : IParameterFactory<RouteParameter> {
        /// <summary>
        /// Optional parameter
        /// </summary>
        /// <returns></returns>
        public RouteParameter Optional() {
            return RouteParameter.Optional;
        }
    }
}