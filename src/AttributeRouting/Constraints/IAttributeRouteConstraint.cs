using System.Collections.Generic;

namespace AttributeRouting.Constraints
{
    public interface IAttributeRouteConstraint
    {
        bool IsMatch(string parameterName, IDictionary<string, object> routeValues);
    }
}