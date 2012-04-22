using System;
using System.Collections.Generic;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a long with a minimum value.
    /// </summary>
    public abstract class MinRouteConstraintBase : IAttributeRouteConstraint
    {
        protected MinRouteConstraintBase(string min)
        {
            var parsedMin = min.ParseLong();
            if (!parsedMin.HasValue)
                throw new ArgumentOutOfRangeException("min", min, "Cannot parse a long from the given value.");

            Min = parsedMin.Value;
        }

        /// <summary>
        /// Minimum value of the parameter.
        /// </summary>
        public long Min { get; private set; }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value.HasNoValue())
                return true;

            var parsedValue = value.ParseLong();
            if (!parsedValue.HasValue)
                return false;

            return parsedValue.Value >= Min;
        }
    }
}