using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string of a length within a given range.
    /// </summary>
    public abstract class RangeLengthRouteConstraintBase : IAttributeRouteConstraint
    {
        // These must be set from the ctor of implementors.
        protected MinLengthRouteConstraintBase MinLengthConstraint;
        protected MaxLengthRouteConstraintBase MaxLengthConstraint;

        /// <summary>
        /// Minimum length of the string.
        /// </summary>
        public int MinLength 
        {
            get { return MinLengthConstraint.MinLength; }
        }

        /// <summary>
        /// Minimum length of the string.
        /// </summary>
        public int MaxLength
        {
            get { return MaxLengthConstraint.MaxLength; }
        }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            return MinLengthConstraint.IsMatch(parameterName, routeValues)
                   && MaxLengthConstraint.IsMatch(parameterName, routeValues);
        }
    }
}