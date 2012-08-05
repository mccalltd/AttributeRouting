using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Abstraction allowing shared matching logic in constraint base implementations.
    /// </summary>
    /// <remarks>
    /// Due to
    /// System.Web.Routing.IRouteConstraint (used in web-hosted scenarios) and 
    /// System.Web.Http.Routing.IHttpRouteConstraint (used in self-hosted scenarios).
    /// </remarks>
    public interface IAttributeRouteConstraint
    {
        bool IsMatch(string parameterName, IDictionary<string, object> routeValues);
    }
}