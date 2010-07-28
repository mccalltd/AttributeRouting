using System;
using System.Collections.Generic;
using System.Reflection;

namespace AttributeRouting
{
    public class AttributeRouteSpecification
    {
        public string AreaName { get; set; }

        public string RoutePrefix { get; set; }

        public Type ControllerType { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public ParameterInfo[] ActionParameters { get; set; }

        public string Url { get; set; }

        public string HttpMethod { get; set; }

        public IEnumerable<RouteDefaultAttribute> DefaultAttributes { get; set; }

        public IEnumerable<RouteConstraintAttribute> ConstraintAttributes { get; set; }

        public string RouteName { get; set; }
    }
}
