using System;
using System.Collections.Generic;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string with a maximum length.
    /// </summary>
    public abstract class MinLengthRouteConstraintBase : IAttributeRouteConstraint
    {
        protected MinLengthRouteConstraintBase(string minLength)
        {
            var parsedMinLength = minLength.ParseInt();
            if (!parsedMinLength.HasValue)
                throw new ArgumentOutOfRangeException("minLength", minLength, "Cannot parse an int from the given value.");

            MinLength = parsedMinLength.Value;
        }

        /// <summary>
        /// Minimum length of the string.
        /// </summary>
        public int MinLength { get; private set; }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value.HasNoValue())
                return true;

            return value.ToString().Length >= MinLength;
        }
    }
}