using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Abstraction used by <see cref="IRouteConstraintFactory"/> 
    /// when handling inline route constraints contained in query strings.
    /// </summary>
    /// <remarks>
    /// Due to 
    /// System.Web.Routing.IRouteConstraint (used in web-hosted scenarios) and 
    /// System.Web.Http.Routing.IHttpRouteConstraint (used in self-hosted scenarios).    
    /// </remarks>
    public interface IQueryStringRouteConstraint
    {
        /// <summary>
        /// The constraint used in the query string.
        /// </summary>
        object Constraint { get; }
    }
}