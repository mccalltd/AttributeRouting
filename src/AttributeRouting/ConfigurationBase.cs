using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
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

            // AutoGenerateRouteNames config setting initialization
            RouteNameBuilder = new FirstInWinsRouteNameBuilder();
        }

        /// <summary>
        /// Factory for generating routes used by AttributeRouting.
        /// </summary>
        public IAttributeRouteFactory AttributeRouteFactory { get; set; }

        /// <summary>
        /// When true, the generated routes will have auto-generated route names in the form controller_action.
        /// The default is false.
        /// </summary>
        public bool AutoGenerateRouteNames { get; set; }

        /// <summary>
        /// Type of the framework controller (IController, IHttpController).
        /// </summary>
        public abstract Type FrameworkControllerType { get; }

        /// <summary>
        /// Collection of available inline route constraint definitions.
        /// </summary>
        public IDictionary<string, Type> InlineRouteConstraints { get; private set; }

        /// <summary>
        /// When true, the generated routes will include actions defined on base controllers.
        /// The default is false.
        /// Note: Base Controllers should be declared as abstract to avoid routes being generated for them
        /// </summary>
        public bool InheritActionsFromBaseController { get; set; }

        /// <summary>
        /// Factory for generating optional route parameters.
        /// </summary>
        public IParameterFactory ParameterFactory { get; set; }

        /// <summary>
        /// Factory for generating route constraints.
        /// </summary>
        public IRouteConstraintFactory RouteConstraintFactory { get; set; }

        /// <summary>
        /// Given a route specification, this delegate returns the route name 
        /// to use when <see cref="AutoGenerateRouteNames"/> is true;
        /// </summary>
        public IRouteNameBuilder RouteNameBuilder { get; set; }

        internal IDictionary<string, object> DefaultRouteConstraints { get; set; }

        internal List<Type> OrderedControllerTypes { get; set; }

        protected void AddDefaultRouteConstraint(string keyRegex, object constraint)
        {
            if (!DefaultRouteConstraints.ContainsKey(keyRegex))
            {
                DefaultRouteConstraints.Add(keyRegex, constraint);
            }
        }

        /// <summary>
        /// Appends the routes from all controllers in the specified assembly to the route collection.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void AddRoutesFromAssembly(Assembly assembly)
        {
            var controllerTypes = assembly.GetControllerTypes(FrameworkControllerType);

            foreach (var controllerType in controllerTypes)
            {
                AddRoutesFromControllerInternal(controllerType);
            }
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
            {
                return;
            }

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
            {
                AddRoutesFromControllerInternal(controllerType, true);
            }
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