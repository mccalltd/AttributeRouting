using System.Collections.Generic;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Abstraction used by <see cref="IRouteConstraintFactory"/> 
    /// when handling compound inline route constraints.
    /// </summary>
    /// <remarks>
    /// Due to 
    /// System.Web.Routing.IRouteConstraint (used in web-hosted scenarios) and 
    /// System.Web.Http.Routing.IHttpRouteConstraint (used in self-hosted scenarios).    
    /// </remarks>
    public interface ICompoundRouteConstraint
    {
        /// <summary>
        /// Constraints to logically and when matching the route parameter.
        /// </summary>
        IEnumerable<object> Constraints { get; }
    }
}
