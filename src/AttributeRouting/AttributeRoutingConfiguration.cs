using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Helpers;

namespace AttributeRouting
{
    /// <summary>
    /// Configuration options to use when mapping AttributeRoutes.
    /// </summary>
    public abstract class AttributeRoutingConfiguration<TRoute, TRequestContext, TRouteData> {

        /// <summary>
        /// Type of the framework controller (IController, IHttpController)
        /// </summary>
        public abstract Type FrameworkControllerType { get; }

        /// <summary>
        /// Creates and initializes a new configuration object.
        /// </summary>
        protected AttributeRoutingConfiguration() {

            InheritActionsFromBaseController = false;
            Assemblies = new List<Assembly>();
            PromotedControllerTypes = new List<Type>();
            DefaultRouteConstraints = new Dictionary<string, object>();            

            TranslationProviders = new List<TranslationProviderBase>();
            CurrentUICultureResolver = (ctx, data) => Thread.CurrentThread.CurrentUICulture.Name;

            AreaSubdomainOverrides = new Dictionary<string, string>();
            DefaultSubdomain = "www";
            SubdomainParser = host =>
            {
                var sections = host.Split('.');
                return sections.Length < 3
                           ? null
                           : String.Join(".", sections.Take(sections.Length - 2));
            };
        }

        /// <summary>
        /// Attribute factory
        /// </summary>
        public abstract IAttributeRouteFactory AttributeFactory { get; }

        /// <summary>
        /// Constraint factory
        /// </summary>
        public abstract IConstraintFactory ConstraintFactory { get; }

        /// <summary>
        /// Parameter factory
        /// </summary>
        public abstract IParameterFactory ParameterFactory { get; } 

        internal List<Assembly> Assemblies { get; set; }
        internal List<Type> PromotedControllerTypes { get; set; }
        internal IDictionary<string, object> DefaultRouteConstraints { get; set; }        
        internal IDictionary<string, string> AreaSubdomainOverrides { get; set; }

        /// <summary>
        /// Translation providers
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
        /// this delegate returns the current UI culture name.
        /// This value is used when constraining inbound routes by culture <see cref="ConstrainTranslatedRoutesByCurrentUICulture"/>.
        /// The default delegate returns the CurrentUICulture name of the current thread.
        /// </summary>
        public Func<TRequestContext, TRouteData, string> CurrentUICultureResolver { get; set; }

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
        public void AddTheRemainingScannedRoutes() { }

        protected void AddDefaultRouteConstraint(string keyRegex, object constraint) {
            if (!DefaultRouteConstraints.ContainsKey(keyRegex))
                DefaultRouteConstraints.Add(keyRegex, constraint);
        }

        /// <summary>
        /// Returns a utility for configuring areas when initializing AttributeRouting framework.
        /// </summary>
        /// <param name="name">The name of the area to configure</param>
        public AreaConfiguration<TRoute, TRequestContext, TRouteData> MapArea(string name)
        {
            return new AreaConfiguration<TRoute, TRequestContext, TRouteData>(name, this);
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
    }
}