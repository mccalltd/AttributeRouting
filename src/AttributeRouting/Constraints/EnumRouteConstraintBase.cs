using System;
using System.Collections.Generic;
using System.Linq;
using AttributeRouting.Helpers;

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
            if (value.HasNoValue())
                return true;

            return _enumNames.Any(n => n.ValueEquals(value.ToString()));
        }
    }
}
