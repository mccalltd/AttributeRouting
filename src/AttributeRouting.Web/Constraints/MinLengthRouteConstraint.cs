using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Constraints
{
    public class MinLengthRouteConstraint : MinLengthRouteConstraintBase, IRouteConstraint
    {
        public MinLengthRouteConstraint(string minLength) : base(minLength) { }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}