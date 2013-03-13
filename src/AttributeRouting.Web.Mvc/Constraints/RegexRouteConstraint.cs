using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Mvc.Constraints
{
    public class RegexRouteConstraint : RegexRouteConstraintBase, IRouteConstraint
    {
        public RegexRouteConstraint(string pattern) : base(pattern) {}

        public RegexRouteConstraint(string pattern, RegexOptions options) 
            : base(pattern, options) {}

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
