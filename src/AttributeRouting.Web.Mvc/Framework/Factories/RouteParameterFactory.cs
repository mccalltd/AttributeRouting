using System.Web.Mvc;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Mvc.Framework.Factories
{
    internal class RouteParameterFactory : IParameterFactory
    {
        public object Optional()
        {
            return UrlParameter.Optional;
        }
    }
}
