using System;
using System.Reflection;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Contains all the information <see cref="RouteBuilder"/> needs to create a route.
    /// </summary>
    public class RouteSpecification
    {
        public MethodInfo ActionMethod { get; set; }
        
        public string ActionName { get; set; }
        
        public long ActionPrecedence { get; set; }
        
        public bool? AppendTrailingSlash { get; set; }
        
        public string AreaName { get; set; }

        public string AreaUrl { get; set; }

        public string AreaUrlTranslationKey { get; set; }
        
        public int ControllerIndex { get; set; }
        
        public string ControllerName { get; set; }
        
        public long ControllerPrecedence { get; set; }
        
        public Type ControllerType { get; set; }

        public string[] HttpMethods { get; set; }
        
        public bool IgnoreAreaUrl { get; set; }
        
        public bool IgnoreRoutePrefix { get; set; }
        
        public bool IsAbsoluteUrl { get; set; }
        
        public long PrefixPrecedence { get; set; }
        
        public bool? PreserveCaseForUrlParameters { get; set; }
        
        public string RoutePrefixUrl { get; set; }

        public string RoutePrefixUrlTranslationKey { get; set; }

        public string RouteUrl { get; set; }

        public string RouteUrlTranslationKey { get; set; }

        public string RouteName { get; set; }

        public long SitePrecedence { get; set; }
        
        public string Subdomain { get; set; }

        public bool? UseLowercaseRoute { get; set; }

        public bool IsVersioned { get; set; }

        public SemanticVersion MinVersion { get; set; }

        public SemanticVersion MaxVersion { get; set; }
    }
}