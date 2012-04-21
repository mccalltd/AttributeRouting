using System;
using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string with a maximum length.
    /// </summary>
    public abstract class MaxLengthRouteConstraintBase : IAttributeRouteConstraint
    {
        protected MaxLengthRouteConstraintBase(string maxLength)
        {
            int maxLengthValue;
            if (int.TryParse(maxLength, out maxLengthValue))
                MaxLength = maxLengthValue;
            else
                throw new ArgumentOutOfRangeException("maxLength", maxLength, "Cannot parse an int from the given value.");
        }

        /// <summary>
        /// Maximum length of the string.
        /// </summary>
        public int MaxLength { get; private set; }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value == null)
                return true;

            return value.ToString().Length <= MaxLength;
        }
    }
}
