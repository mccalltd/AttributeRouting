using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Constraints
{
    public class RangeRouteConstraint : RangeRouteConstraintBase, IRouteConstraint
    {
        public RangeRouteConstraint(string min, string max)
        {
            MinConstraint = new MinRouteConstraint(min);
            MaxConstraint = new MaxRouteConstraint(max);
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}