using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a long with a maximum value.
    /// </summary>
    public class MaxRouteConstraint : MaxRouteConstraintBase, IRouteConstraint
    {
        public MaxRouteConstraint(string max) : base (max) {}

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}