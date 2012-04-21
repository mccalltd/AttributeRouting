using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Constraints
{
    /// <summary>
    /// Constraints a url parameter by a regular expression.
    /// </summary>
    public class RegexRouteConstraint : RegexRouteConstraintBase, IRouteConstraint
    {
        public RegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) 
            : base(pattern, options) {}

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
