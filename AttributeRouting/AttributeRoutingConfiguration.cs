using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace AttributeRouting
{
    public class AttributeRoutingConfiguration
    {
        public AttributeRoutingConfiguration()
        {
            AddScannedRoutes = true;
            UseLowercaseRoutes = false;
            ConstrainPrimitiveRouteParameters = true;

            Assemblies = new List<Assembly>();
            PromotedControllerTypes = new List<Type>();
        }

        internal List<Assembly> Assemblies { get; set; }
        internal List<Type> PromotedControllerTypes { get; set; }
        internal bool AddScannedRoutes { get; set; }
        
        /// <summary>
        /// When true, the route generator will automatically add route constraints 
        /// for URL parameters that map to action parameters with primitive types.
        /// The default is true.
        /// </summary>
        public bool ConstrainPrimitiveRouteParameters { get; set; }
        
        /// <summary>
        /// When true, the generated routes will produce lowercase outbound URLs.
        /// The default is false.
        /// </summary>
        public bool UseLowercaseRoutes { get; set; }

        /// <summary>
        /// Scans the assembly of the specified controller for routes to register.
        /// </summary>
        /// <typeparam name="TController">The controller type used to specify the assembly</typeparam>
        public void ScanAssemblyOf<TController>()
        {
            ScanAssembly(typeof(TController).Assembly);
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
        /// <typeparam name="TController">The base controller type</typeparam>
        public void AddRoutesFromControllersOfType<TController>()
        {
            var baseControllerType = typeof(TController);
            var assembly = baseControllerType.Assembly;

            var controllerTypes = from controllerType in assembly.GetControllerTypes()
                                  where baseControllerType.IsAssignableFrom(controllerType)
                                  select controllerType;

            foreach (var controllerType in controllerTypes)
                AddRoutesFromController(controllerType);
        }

        /// <summary>
        /// Adds all the routes for the specified controller to the end of the route collection.
        /// </summary>
        /// <typeparam name="TController">The controller type</typeparam>
        public void AddRoutesFromController<TController>()
            where TController : Controller
        {
            AddRoutesFromController(typeof(TController));
        }

        private void AddRoutesFromController(Type controllerType)
        {
            AddScannedRoutes = false;

            if (!PromotedControllerTypes.Contains(controllerType))
                PromotedControllerTypes.Add(controllerType);            
        }

        /// <summary>
        /// When using AddRoutesFromControllersOfType or AddRoutesFromController to set the precendence of the routes,
        /// you must explicitly specify that you want to include the remaining routes discoved while scanning assemblies.
        /// </summary>
        public void AddTheRemainingScannedRoutes()
        {
            AddScannedRoutes = true;
        }
    }
}
