using System.Web.Http;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost.Framework.Factories
{
    internal class RouteParameterFactory : IParameterFactory
    {
        /// <summary>
        /// Optional parameter
        /// </summary>
        /// <returns></returns>
        public object Optional()
        {
            return RouteParameter.Optional;
        }
    }
}