using System.Web.Http;
using AttributeRouting.Framework;

namespace AttributeRouting.Web.Http.Framework
{
    internal class RouteParameterFactory : IParameterFactory
    {
        public object Optional()
        {
            return RouteParameter.Optional;
        }
    }
}