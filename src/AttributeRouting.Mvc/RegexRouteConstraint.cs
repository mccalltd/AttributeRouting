using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting.Mvc
{
    /// <summary>
    /// Applies a regex constraint against the associated url parameter.
    /// </summary>
    public class RegexRouteConstraint : RegexRouteConstraintBase, IRouteConstraint
    {
        public RegexRouteConstraint(string pattern) : base(pattern) {}

        public RegexRouteConstraint(string pattern, RegexOptions options) : base(pattern, options) {}

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            var value = values[parameterName];
            if (value == null)
                return true;

            var valueAsString = value.ToString();

            return Regex.IsMatch(valueAsString, Pattern, Options);
        }
    }
}