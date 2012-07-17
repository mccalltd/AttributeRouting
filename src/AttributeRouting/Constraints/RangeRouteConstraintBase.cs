using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a long within a given range of values.
    /// </summary>
    public abstract class RangeRouteConstraintBase : IAttributeRouteConstraint
    {
        // These must be set from the ctor of implementors.
        protected MinRouteConstraintBase MinConstraint;
        protected MaxRouteConstraintBase MaxConstraint;

        /// <summary>
        /// Minimum value.
        /// </summary>
        public long Min 
        {
            get { return MinConstraint.Min; }
        }

        /// <summary>
        /// Minimum valut.
        /// </summary>
        public long Max
        {
            get { return MaxConstraint.Max; }
        }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            return MinConstraint.IsMatch(parameterName, routeValues)
                   && MaxConstraint.IsMatch(parameterName, routeValues);
        }
    }
}