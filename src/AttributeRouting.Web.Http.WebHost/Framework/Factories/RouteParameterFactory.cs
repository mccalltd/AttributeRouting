using System.Web.Http;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost.Framework.Factories
{
    internal class RouteParameterFactory : IRouteDefaultFactory
    {
        public object Optional()
        {
            return RouteParameter.Optional;
        }
    }
}
