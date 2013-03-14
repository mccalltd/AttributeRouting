using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Mvc.Constraints
{
    public class MaxLengthRouteConstraint : MaxLengthRouteConstraintBase, IRouteConstraint
    {
        public MaxLengthRouteConstraint(string maxLength) : base(maxLength) { }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
