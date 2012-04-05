using System;
using System.Collections.Generic;
using System.Reflection;

namespace AttributeRouting.Framework
{
    public class RouteSpecification<TConstraint>
    {
        public RouteSpecification()
        {
            ActionParameters = new ParameterInfo[0];
            DefaultAttributes = new List<RouteDefaultAttribute>();
            ConstraintAttributes = new List<IRouteConstraint<TConstraint>>();
        }

        public string AreaName { get; set; }

        public string AreaUrl { get; set; }

        public string AreaUrlTranslationKey { get; set; }

        public string RoutePrefixUrl { get; set; }

        public string RoutePrefixUrlTranslationKey { get; set; }

        public Type ControllerType { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public ParameterInfo[] ActionParameters { get; set; }

        public string RouteUrl { get; set; }

        public string RouteUrlTranslationKey { get; set; }

        public string[] HttpMethods { get; set; }

        public ICollection<RouteDefaultAttribute> DefaultAttributes { get; set; }

        public ICollection<IRouteConstraint<TConstraint>> ConstraintAttributes { get; set; }

        public string RouteName { get; set; }

        public bool IsAbsoluteUrl { get; set; }

        public string Subdomain { get; set; }
    }
}