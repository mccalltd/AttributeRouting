using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.WebApi.Framework.Factories
{
    internal class RouteParameterFactory : IParameterFactory<RouteParameter>
    {
        public RouteParameter Optional()
        {
            return RouteParameter.Optional;
        }
    }
}
