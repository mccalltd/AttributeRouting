using System.Web.Http;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Http.WebHost.Framework.Factories
{
    internal class RouteParameterFactory : IParameterFactory<RouteParameter>
    {
        public RouteParameter Optional()
        {
            return RouteParameter.Optional;
        }
    }
}
