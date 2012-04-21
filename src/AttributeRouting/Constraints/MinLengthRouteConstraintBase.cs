using System;
using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string with a maximum length.
    /// </summary>
    public abstract class MinLengthRouteConstraintBase : IAttributeRouteConstraint
    {
        protected MinLengthRouteConstraintBase(string minLength)
        {
            int minLengthValue;
            if (int.TryParse(minLength, out minLengthValue))
                MinLength = minLengthValue;
            else
                throw new ArgumentOutOfRangeException("minLength", minLength, "Cannot parse an int from the given value.");
        }

        /// <summary>
        /// Minimum length of the string.
        /// </summary>
        public int MinLength { get; private set; }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value == null)
                return true;

            return value.ToString().Length >= MinLength;
        }
    }
}