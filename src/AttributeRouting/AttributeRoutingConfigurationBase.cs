using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Helpers;

namespace AttributeRouting
{
    /// <summary>
    /// Configuration options to use when generating AttributeRoutes.
    /// </summary>
    public abstract class AttributeRoutingConfigurationBase
    {
        private readonly List<string> _registeredRouteNames = new List<string>();

        /// <summary>
        /// Creates and initializes a new configuration object.
        /// </summary>
        protected AttributeRoutingConfigurationBase()
        {
            Assemblies = new List<Assembly>();
            PromotedControllerTypes = new List<Type>();

            InheritActionsFromBaseController = false;

            // Constraint setting initialization
            DefaultRouteConstraints = new Dictionary<string, object>();
            InlineRouteConstraints = new Dictionary<string, Type>();

            // Translation setting initialization
            TranslationProviders = new List<TranslationProviderBase>();

            // Subdomain config setting initialization
            AreaSubdomainOverrides = new Dictionary<string, string>();
            DefaultSubdomain = "www";
            SubdomainParser = host =>
            {
                var sections = host.Split('.');
                return sections.Length < 3
                           ? null
                           : String.Join(".", sections.Take(sections.Length - 2));
            };

            // AutoGenerateRouteNames config setting initialization
            RouteNameBuilder = routeSpec =>
            {
                var areaPart = routeSpec.AreaName.HasValue() ? "{0}_".FormatWith(routeSpec.AreaName) : null;
                var routeName = "{0}{1}_{2}".FormatWith(areaPart, routeSpec.ControllerName, routeSpec.ActionName);

                // Only register route names once, so first in wins.
                if (!_registeredRouteNames.Contains(routeName))
                {
                    _registeredRouteNames.Add(routeName);
                    return routeName;
                }

                return null;
            };
        }

        /// <summary>
        /// Type of the framework controller (IController, IHttpController).
        /// </summary>
        public abstract Type FrameworkControllerType { get; }

        /// <summary>
        /// Factory for generating routes used by AttributeRouting.
        /// </summary>
        public abstract IAttributeRouteFactory AttributeFactory { get; }

        /// <summary>
        /// Factory for generating route constraints.
        /// </summary>
        public abstract IRouteConstraintFactory RouteConstraintFactory { get; }

        /// <summary>
        /// Factory for generating optional route parameters.
        /// </summary>
        public abstract IParameterFactory ParameterFactory { get; }
        
        internal List<Assembly> Assemblies { get; set; }
        
        internal List<Type> PromotedControllerTypes { get; set; }
        
        internal IDictionary<string, object> DefaultRouteConstraints { get; set; }
        
        internal IDictionary<string, string> AreaSubdomainOverrides { get; set; }

        /// <summary>
        /// Collection of available inline route constraint definitions.
        /// </summary>
        public IDictionary<string, Type> InlineRouteConstraints { get; private set; }

        /// <summary>
        /// Translation providers.
        /// </summary>
        public List<TranslationProviderBase> TranslationProviders { get; set; }

        /// <summary>
        /// When true, the generated routes will produce lowercase URLs.
        /// The default is false.
        /// </summary>
        public bool UseLowercaseRoutes { get; set; }

        /// <summary>
        /// When true, the generated routes will not lowercase URL parameter values.
        /// The default is false.
        /// </summary>
        public bool PreserveCaseForUrlParameters { get; set; }

        /// <summary>
        /// When true, the generated routes will have a trailing slash on the path of outbound URLs.
        /// The default is false.
        /// </summary>
        public bool AppendTrailingSlash { get; set; }

        /// <summary>
        /// When true, the generated routes will have auto-generated route names in the form controller_action.
        /// The default is false.
        /// </summary>
        public bool AutoGenerateRouteNames { get; set; }

        /// <summary>
        /// Given a route specification, this delegate returns the route name 
        /// to use when <see cref="AutoGenerateRouteNames"/> is true;
        /// </summary>
        public Func<RouteSpecification, string> RouteNameBuilder { get; set; }

        /// <summary>
        /// Given the requested hostname, this delegate parses the subdomain.
        /// The default yields everything before the domain name;
        /// eg: www.example.com yields www, and example.com yields null.
        /// </summary>
        public Func<string, string> SubdomainParser { get; set; }

        /// <summary>
        /// Specify the default subdomain for this application.
        /// The default is www.
        /// </summary>
        public string DefaultSubdomain { get; set; }

        /// <summary>
        /// When true, the generated routes will include actions defined on base controllers.
        /// The default is false.
        /// Note: Base Controllers should be declared as abstract to avoid routes being generated for them
        /// </summary>
        public bool InheritActionsFromBaseController { get; set; }

        /// <summary>
        /// Constrains translated routes by the thread's current UI culture.
        /// The default is false.
        /// </summary>
        public bool ConstrainTranslatedRoutesByCurrentUICulture { get; set; }

        /// <summary>
        /// Returns a utility for configuring areas when initializing AttributeRouting framework.
        /// </summary>
        /// <param name="name">The name of the area to configure</param>
        public AreaConfiguration MapArea(string name)
        {
            return new AreaConfiguration(name, this);
        }

        /// <summary>
        /// Scans the specified assembly for routes to register.
        /// </summary>
        /// <param name="assembly">The assembly</param>
        public void ScanAssembly(Assembly assembly)
        {
            if (!Assemblies.Contains(assembly))
                Assemblies.Add(assembly);
        }

        /// <summary>
        /// Adds all the routes for all the controllers that derive from the specified controller
        /// to the end of the route collection.
        /// </summary>
        /// <param name="baseControllerType">The base controller type</param>
        public void AddRoutesFromControllersOfType(Type baseControllerType)
        {
            var assembly = baseControllerType.Assembly;

            var controllerTypes = from controllerType in assembly.GetControllerTypes(FrameworkControllerType)
                                  where baseControllerType.IsAssignableFrom(controllerType)
                                  select controllerType;

            foreach (var controllerType in controllerTypes)
                AddRoutesFromController(controllerType);
        }

        /// <summary>
        /// Adds all the routes for the specified controller type to the end of the route collection.
        /// </summary>
        /// <param name="controllerType">The controller type</param>
        public void AddRoutesFromController(Type controllerType)
        {
            if (!FrameworkControllerType.IsAssignableFrom(controllerType))
                return;

            if (!PromotedControllerTypes.Contains(controllerType))
                PromotedControllerTypes.Add(controllerType);
        }

        /// <summary>
        /// When using AddRoutesFromControllersOfType or AddRoutesFromController to set the precendence of the routes,
        /// you must explicitly specify that you want to include the remaining routes discoved while scanning assemblies.
        /// </summary>
        [Obsolete("This is a vestigial workaround for an old assembly scanning issue.")]
        public void AddTheRemainingScannedRoutes() {}

        protected void AddDefaultRouteConstraint(string keyRegex, object constraint)
        {
            if (!DefaultRouteConstraints.ContainsKey(keyRegex))
                DefaultRouteConstraints.Add(keyRegex, constraint);
        }

        /// <summary>
        /// Add a provider for translating components of routes.
        /// </summary>
        public void AddTranslationProvider<TTranslationProvider>()
            where TTranslationProvider : TranslationProviderBase, new()
        {
            TranslationProviders.Add(new TTranslationProvider());
        }

        /// <summary>
        /// Add a provider for translating components of routes.
        /// Use <see cref="FluentTranslationProvider"/> for a default implementation.
        /// </summary>
        public void AddTranslationProvider(TranslationProviderBase provider)
        {
            TranslationProviders.Add(provider);
        }

        internal IEnumerable<string> GetTranslationProviderCultureNames()
        {
            return (from provider in TranslationProviders
                    from cultureName in provider.CultureNames
                    select cultureName).Distinct().ToList();
        }

        protected void RegisterDefaultInlineRouteConstraints<TRouteConstraint>(Assembly assembly)
        {
            // Register default inline route constraints
            var inlineConstraintTypes = from t in assembly.GetTypes()
                                        where typeof(TRouteConstraint).IsAssignableFrom(t)
                                              && typeof(IAttributeRouteConstraint).IsAssignableFrom(t)
                                        select t;

            foreach (var inlineConstraintType in inlineConstraintTypes)
            {
                var name = Regex.Replace(inlineConstraintType.Name, "RouteConstraint$", "").ToLowerInvariant();
                InlineRouteConstraints.Add(name, inlineConstraintType);
            }
        }
    }
}