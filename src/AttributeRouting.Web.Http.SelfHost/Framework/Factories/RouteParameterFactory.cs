using System.Web.Http;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost.Framework.Factories {
    internal class RouteParameterFactory : IParameterFactory<RouteParameter> {
        /// <summary>
        /// Optional parameter
        /// </summary>
        /// <returns></returns>
        public RouteParameter Optional() {
            return RouteParameter.Optional;
        }
    }
}