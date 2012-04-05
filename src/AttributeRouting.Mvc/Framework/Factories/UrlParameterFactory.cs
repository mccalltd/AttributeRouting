using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Mvc.Framework.Factories
{
    internal class UrlParameterFactory : IParameterFactory<UrlParameter>
    {
        public UrlParameter Optional()
        {
            return UrlParameter.Optional;
        }
    }
}
