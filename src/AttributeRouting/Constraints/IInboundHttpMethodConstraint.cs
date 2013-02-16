using System.Collections.Generic;
using AttributeRouting.Framework;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Abstraction used by <see cref="IRouteConstraintFactory"/> 
    /// when applying inbound http method constraints.
    /// </summary>
    /// <remarks>
    /// Due to 
    /// System.Web.Routing.HttpMethodConstraint (used in web-hosted scenarios) and
    /// System.Web.Http.Routing.HttpMethodConstraint (used in self-hosted scenarios).    
    /// </remarks>
    public interface IInboundHttpMethodConstraint
    {
        /// <summary>
        /// The allowed HTTP methods.
        /// </summary>
        ICollection<string> AllowedMethods { get; }
    }
}