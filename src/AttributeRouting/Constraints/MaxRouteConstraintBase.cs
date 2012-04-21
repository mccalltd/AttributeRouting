using System;
using System.Collections.Generic;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a long with a maximum value.
    /// </summary>
    public abstract class MaxRouteConstraintBase : IAttributeRouteConstraint
    {
        protected MaxRouteConstraintBase(string max)
        {
            var parsedMax = max.ParseLong();
            if (!parsedMax.HasValue)
                throw new ArgumentOutOfRangeException("max", max, "Cannot parse a long from the given value.");

            Max = parsedMax.Value;
        }

        /// <summary>
        /// Maximum value of the parameter.
        /// </summary>
        public long Max { get; private set; }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value == null)
                return true;

            var parsedValue = value.ParseLong();
            if (!parsedValue.HasValue)
                return false;

            return parsedValue.Value <= Max;
        }
    }
}