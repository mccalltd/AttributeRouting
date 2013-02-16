using System.Web.Mvc;
using AttributeRouting.Framework;

namespace AttributeRouting.Web.Mvc.Framework
{
    internal class RouteParameterFactory : IParameterFactory
    {
        public object Optional()
        {
            return UrlParameter.Optional;
        }
    }
}
