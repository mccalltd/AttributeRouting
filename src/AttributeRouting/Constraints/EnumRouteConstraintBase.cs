using System;
using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a value from an enum.
    /// </summary>
    public abstract class EnumRouteConstraintBase<T> : IAttributeRouteConstraint 
        where T : struct
    {
        private readonly HashSet<string> _enumNames;

        protected EnumRouteConstraintBase()
        {
            _enumNames = new HashSet<string>(Enum.GetNames(typeof(T)));
        }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value == null)
                return true;

            return _enumNames.Contains(value.ToString());
        }
    }
}
