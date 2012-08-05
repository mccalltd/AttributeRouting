using System.Collections.Generic;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Abstraction used by <see cref="IRouteConstraintFactory"/> 
    /// when applying restful route constraints.
    /// </summary>
    /// <remarks>
    /// Due to 
    /// System.Web.Routing.HttpMethodConstraint (used in web-hosted scenarios) and
    /// System.Web.Http.Routing.HttpMethodConstraint (used in self-hosted scenarios).    
    /// </remarks>
    public interface IRestfulHttpMethodConstraint
    {
        /// <summary>
        /// The allowed HTTP methods.
        /// </summary>
        ICollection<string> AllowedMethods { get; }
    }
}