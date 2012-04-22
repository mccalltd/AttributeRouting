using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Constraints
{
    public class AlphaRouteConstraint : AlphaRouteConstraintBase, IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
