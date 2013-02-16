using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Abstraction used by <see cref="IRouteConstraintFactory"/> 
    /// when handling optional route parameters with inline constraints.
    /// </summary>
    /// <remarks>
    /// Due to 
    /// System.Web.Routing.IRouteConstraint (used in web-hosted scenarios) and 
    /// System.Web.Http.Routing.IHttpRouteConstraint (used in self-hosted scenarios).    
    /// </remarks>
    public interface IOptionalRouteConstraint
    {
        /// <summary>
        /// Constraint to consider optional.
        /// </summary>
        object Constraint { get; }
    }
}
