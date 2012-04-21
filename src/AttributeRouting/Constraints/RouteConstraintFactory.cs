using System;
using System.Collections.Generic;
using System.Reflection;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    public static class RouteConstraintFactory
    {
        // Constraint types cache
        private static readonly Dictionary<string, Type> ConstraintTypes 
            = new Dictionary<string, Type>();

        // Constraint instances cache, only for constraints that do not have any parameters
        private static readonly Dictionary<string, IAttributeRouteConstraint> ConstraintInstances 
            = new Dictionary<string, IAttributeRouteConstraint>();

        private static Type GetConstraintTypeFromCache(string constraintTypeName)
        {
            // Check in the cache first
            if (ConstraintTypes.ContainsKey(constraintTypeName))
                return ConstraintTypes[constraintTypeName];

            // Get the type that implements this constraint
            var type = Assembly
                .GetExecutingAssembly()
                .GetType("AttributeRouting.Constraints." + constraintTypeName + "RouteConstraint", false, true);

            if (type == null)
                throw new AttributeRoutingException("Unknown constraint type: " + constraintTypeName);

            return ConstraintTypes[constraintTypeName] = type;
        }

        private static IAttributeRouteConstraint GetConstraintInstanceFromCache(string constraintTypeName)
        {
            // Check in the cache first
            if (ConstraintInstances.ContainsKey(constraintTypeName))
                return ConstraintInstances[constraintTypeName];

            // Get the type that implements this constraint
            var type = GetConstraintTypeFromCache(constraintTypeName);

            // Create an instance of it
            var instance = (IAttributeRouteConstraint)Activator.CreateInstance(type);

            return ConstraintInstances[constraintTypeName] = instance;
        }

        public static IAttributeRouteConstraint GetConstraint(string constraintTypeName, params object[] constraintParameters)
        {
            if (constraintParameters.Length == 0)
                return GetConstraintInstanceFromCache(constraintTypeName);

            try
            {
                return (IAttributeRouteConstraint)Activator.CreateInstance(GetConstraintTypeFromCache(constraintTypeName), constraintParameters);
            }
            catch (MissingMethodException)
            {
                // No constructor with that number of arguments
                throw new AttributeRoutingException(
                    "Invalid parameter count ({0}) for the '{1}' constraint: {2}"
                        .FormatWith(constraintParameters.Length,
                                    constraintTypeName,
                                    String.Join(",", constraintParameters)));
            }
        }

        public static void RegisterConstraintType(string constraintTypeName, Type type)
        {
            if (!typeof(IAttributeRouteConstraint).IsAssignableFrom(type))
                throw new ArgumentException("type must implement IAttributeRouteConstraint", "type");

            ConstraintTypes[constraintTypeName] = type;
        }
    }
}
