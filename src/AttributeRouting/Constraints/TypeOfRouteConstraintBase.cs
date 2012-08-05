using System.Collections.Generic;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be parsable as the given type.
    /// </summary>
    public abstract class TypeOfRouteConstraintBase<T> : IAttributeRouteConstraint
        where T : struct
    {
        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value.HasNoValue())
                return true;

            var parsedValue = value.Parse<T>();
            return parsedValue.HasValue;
        }
    }
}