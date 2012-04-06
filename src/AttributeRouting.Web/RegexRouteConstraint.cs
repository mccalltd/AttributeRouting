using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web
{
    /// <summary>
    /// Applies a regex constraint against the associated url parameter.
    /// </summary>
    public class RegexRouteConstraint : RegexRouteConstraintBase, IRouteConstraint
    {
        public RegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
            : base(pattern, options)
        {
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return base.IsMatch(parameterName, values);
        }
    }
}