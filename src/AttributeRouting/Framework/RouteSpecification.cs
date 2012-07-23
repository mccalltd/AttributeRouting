using System;
using System.Collections.Generic;

namespace AttributeRouting.Framework
{
    public class RouteSpecification
    {
        public RouteSpecification()
        {
            DefaultAttributes = new List<RouteDefaultAttribute>();
            ConstraintAttributes = new List<RouteConstraintAttributeBase>();
        }

        public string AreaName { get; set; }

        public string AreaUrl { get; set; }

        public string AreaUrlTranslationKey { get; set; }

        public string RoutePrefixUrl { get; set; }

        public string RoutePrefixUrlTranslationKey { get; set; }

        public Type ControllerType { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string RouteUrl { get; set; }

        public string RouteUrlTranslationKey { get; set; }

        public string[] HttpMethods { get; set; }

        public ICollection<RouteDefaultAttribute> DefaultAttributes { get; set; }

        public ICollection<RouteConstraintAttributeBase> ConstraintAttributes { get; set; }

        public string RouteName { get; set; }

        public bool IsAbsoluteUrl { get; set; }

        public string Subdomain { get; set; }

        public bool? UseLowercaseRoute { get; set; }

        public bool? PreserveCaseForUrlParameters { get; set; }

        public bool? AppendTrailingSlash { get; set; }

        public bool IsVersioned { get; set; }

        public SemanticVersion MinVersion { get; set; }

        public SemanticVersion MaxVersion { get; set; }
    }
}