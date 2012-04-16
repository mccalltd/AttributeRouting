using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// This is an interface for logging and testing; concrete implementations inherit from their respective
    /// framework constraints (System.Web.Routing, System.Web.Http.Routing)
    /// </summary>
    public interface IRestfulHttpMethodConstraint
    {
        /// <summary>
        /// The allowed HTTP methods
        /// </summary>
        ICollection<string> AllowedMethods { get; }
    }
}