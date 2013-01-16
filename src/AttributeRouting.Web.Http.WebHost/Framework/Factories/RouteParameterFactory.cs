using System.Web.Http;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost.Framework.Factories
{
    internal class RouteParameterFactory : IParameterFactory
    {
        public object Optional()
        {
            return RouteParameter.Optional;
        }
    }
}
