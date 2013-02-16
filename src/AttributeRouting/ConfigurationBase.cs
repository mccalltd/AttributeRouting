using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Helpers;

namespace AttributeRouting
{
    /// <summary>
    /// Configuration options to use when generating AttributeRoutes.
    /// </summary>
    public abstract class ConfigurationBase
    {
        /// <summary>
        /// Creates and initializes a new configuration object.
        /// </summary>
        protected ConfigurationBase()
        {
            OrderedControllerTypes = new List<Type>();

            InheritActionsFromBaseController = false;

            // Constraint setting initialization
            DefaultRouteConstraints = new Dictionary<string, object>();
            InlineRouteConstraints = new Dictionary<string, Type>();

            // Translation setting initialization
            TranslationProviders = new List<TranslationProviderBase>();

            // Subdomain config setting initialization
            AreaSubdomainOverrides = new Dictionary<string, string>();
            DefaultSubdomain = "www";
            SubdomainParser = SubdomainParsers.ThreeSection;

            // AutoGenerateRouteNames config setting initialization
            RouteNameBuilder = RouteNameBuilders.FirstInWins;
        }

        internal List<Type> OrderedControllerTypes { get; set; }

        internal IDictionary<string, object> DefaultRouteConstraints { get; set; }

        internal IDictionary<string, string> AreaSubdomainOverrides { get; set; }

        /// <summary>
        /// Type of the framework controller (IController, IHttpController).
        /// </summary>
        public abstract Type FrameworkControllerType { get; }

        /// <summary>
        /// Factory for generating routes used by AttributeRouting.
        /// </summary>
        public IAttributeRouteFactory AttributeRouteFactory { get; set; }

        /// <summary>
        /// Factory for generating route constraints.
        /// </summary>
        public IRouteConstraintFactory RouteConstraintFactory { get; set; }

        /// <summary>
        /// Factory for generating optional route parameters.
        /// </summary>
        public IParameterFactory ParameterFactory { get; set; }
        
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
        /// Scans the assembly of the specified controller for routes to register.
        /// </summary>
        /// <typeparam name="T">The type used to specify the assembly.</typeparam>
        [Obsolete("Prefer using AddRoutesFromController, AddRoutesFromControllersOfType, and AddRoutesFromAssembly.")]
        public void ScanAssemblyOf<T>()
        {
            ScanAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Scans the specified assembly for routes to register.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        [Obsolete("Prefer using AddRoutesFromController, AddRoutesFromControllersOfType, and AddRoutesFromAssembly.")]
        public void ScanAssembly(Assembly assembly)
        {
            AddRoutesFromAssembly(assembly);
        }

        /// <summary>
        /// Appends the routes from all controllers in the specified assembly to the route collection.
        /// </summary>
        /// <typeparam name="T">The type denoting the assembly.</typeparam>
        public void AddRoutesFromAssemblyOf<T>()
        {
            AddRoutesFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Appends the routes from all controllers in the specified assembly to the route collection.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void AddRoutesFromAssembly(Assembly assembly)
        {
            var controllerTypes = assembly.GetControllerTypes(FrameworkControllerType);

            foreach (var controllerType in controllerTypes)
                AddRoutesFromControllerInternal(controllerType);
        }

        /// <summary>
        /// Appends the routes from all controllers that derive from the specified controller type to the route collection.
        /// </summary>
        /// <param name="baseControllerType">The base controller type.</param>
        public void AddRoutesFromControllersOfType(Type baseControllerType)
        {
            var assembly = baseControllerType.Assembly;

            var controllerTypes = from controllerType in assembly.GetControllerTypes(FrameworkControllerType)
                                  where baseControllerType.IsAssignableFrom(controllerType)
                                  select controllerType;

            foreach (var controllerType in controllerTypes)
                AddRoutesFromControllerInternal(controllerType, true);
        }

        /// <summary>
        /// Appends the routes from the specified controller type to the end of route collection.
        /// </summary>
        /// <param name="controllerType">The controller type.</param>
        public void AddRoutesFromController(Type controllerType)
        {
            AddRoutesFromControllerInternal(controllerType, true);
        }

        /// <summary>
        /// Appends the routes from the controller to the promoted controller type list,
        /// optionally removing an already added type in order to add it to the end of the list.
        /// </summary>
        /// <param name="controllerType">The controller type.</param>
        /// <param name="reorderTypes">Whether to remove and re-add already added controller types.</param>
        private void AddRoutesFromControllerInternal(Type controllerType, bool reorderTypes = false)
        {
            if (!FrameworkControllerType.IsAssignableFrom(controllerType))
                return;

            if (!OrderedControllerTypes.Contains(controllerType))
            {
                OrderedControllerTypes.Add(controllerType);
            }
            else if (reorderTypes)
            {
                OrderedControllerTypes.Remove(controllerType);
                OrderedControllerTypes.Add(controllerType);
            }
        }

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