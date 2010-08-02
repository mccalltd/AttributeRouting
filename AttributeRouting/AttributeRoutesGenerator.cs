using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttributeRouting
{
    public class AttributeRoutesGenerator
    {
        private readonly AttributeRoutingConfiguration _configuration;

        public AttributeRoutesGenerator(AttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<AttributeRoute> Generate()
        {
            var routeSpecificationGenerator = new RouteReflector(_configuration);
            var routeSpecs = routeSpecificationGenerator.GenerateRouteSpecifications();

            var attributeRouteBuilder = new AttributeRouteBuilder(_configuration);
            return routeSpecs.Select(attributeRouteBuilder.Build);
        }
    }
}
