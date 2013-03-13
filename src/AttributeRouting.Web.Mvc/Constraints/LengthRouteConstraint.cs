using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Mvc.Constraints
{
    public class LengthRouteConstraint : LengthRouteConstraintBase, IRouteConstraint
    {
        public LengthRouteConstraint(string length) : base(length) {}

        public LengthRouteConstraint(string minLength, string maxLength)
            : base(new MinLengthRouteConstraint(minLength),
                   new MaxLengthRouteConstraint(maxLength)) {}

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}