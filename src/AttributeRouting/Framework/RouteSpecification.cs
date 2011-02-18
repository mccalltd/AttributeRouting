using System;
using System.Collections.Generic;
using System.Reflection;

namespace AttributeRouting.Framework
{
    public class RouteSpecification
    {
        public RouteSpecification()
        {
            ActionParameters = new ParameterInfo[0];
            DefaultAttributes = new List<RouteDefaultAttribute>();
            ConstraintAttributes = new List<RouteConstraintAttribute>();
        }

        public string AreaName { get; set; }

        public string AreaUrl { get; set; }

        public string RoutePrefix { get; set; }

        public Type ControllerType { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public ParameterInfo[] ActionParameters { get; set; }

        public string Url { get; set; }

        public string HttpMethod { get; set; }

        public ICollection<RouteDefaultAttribute> DefaultAttributes { get; set; }

        public ICollection<RouteConstraintAttribute> ConstraintAttributes { get; set; }

        public string RouteName { get; set; }

        public bool IsAbsoluteUrl { get; set; }
    }
}