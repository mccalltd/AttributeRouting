using System.Web.Mvc;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Mvc.Framework.Factories
{
    internal class UrlParameterFactory : IParameterFactory<UrlParameter>
    {
        public UrlParameter Optional()
        {
            return UrlParameter.Optional;
        }
    }
}
