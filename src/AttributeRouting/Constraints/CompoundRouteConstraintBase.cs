using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Wraps compound constraints speicifed inline to allow for anding.
    /// </summary>
    /// <remarks>
    /// Supports constraints specified like: param:constraint1:constraint2.
    /// </remarks>
    public interface ICompoundRouteConstraint
    {
        /// <summary>
        /// Constraints to logically and when matching the route parameter.
        /// </summary>
        IEnumerable<object> Constraints { get; }
    }
}
