using System.Web.Mvc;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Mvc.Framework.Factories
{
    internal class RouteParameterFactory : IRouteDefaultFactory
    {
        public object Optional()
        {
            return UrlParameter.Optional;
        }
    }
}
