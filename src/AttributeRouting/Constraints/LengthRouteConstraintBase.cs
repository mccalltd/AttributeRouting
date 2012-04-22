using System;
using System.Collections.Generic;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string of a given length 
    /// or within a given range of lengths if two params are given.
    /// </summary>
    public abstract class LengthRouteConstraintBase : IAttributeRouteConstraint
    {
        // These must be set from the ctor of implementors.
        protected MinLengthRouteConstraintBase MinLengthConstraint;
        protected MaxLengthRouteConstraintBase MaxLengthConstraint;

        /// <summary>
        /// Constraints a url parameter to be a string of a given length.
        /// </summary>
        /// <param name="length">The length of the string</param>
        protected LengthRouteConstraintBase(string length)
        {
            var parsedLength = length.ParseInt();
            if (!parsedLength.HasValue)
                throw new ArgumentOutOfRangeException("length", length, "Cannot parse an int from the given value.");

            Length = parsedLength;
        }

        /// <summary>
        /// Constraints a url parameter to be within a given range of lengths.
        /// </summary>
        /// <param name="minLength">The minimum length constraint</param>
        /// <param name="maxLength">The maximum length constraint</param>
        protected LengthRouteConstraintBase(MinLengthRouteConstraintBase minLength, MaxLengthRouteConstraintBase maxLength)
        {
            MinLengthConstraint = minLength;
            MaxLengthConstraint = maxLength;
        }

        /// <summary>
        /// Length of the string.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Minimum length of the string.
        /// </summary>
        public int? MinLength 
        {
            get { return MinLengthConstraint != null ? MinLengthConstraint.MinLength : (int?)null; }
        }

        /// <summary>
        /// Minimum length of the string.
        /// </summary>
        public int? MaxLength
        {
            get { return MaxLengthConstraint.MaxLength; }
        }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            if (Length.HasValue)
            {
                var value = routeValues[parameterName];
                if (value.HasNoValue())
                    return true;

                return value.ToString().Length == Length.Value;
            }
            
            return MinLengthConstraint.IsMatch(parameterName, routeValues)
                   && MaxLengthConstraint.IsMatch(parameterName, routeValues);
        }
    }
}