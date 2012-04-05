using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting.Mvc
{
    /// <summary>
    /// Applies a regex constraint against the associated url parameter.
    /// </summary>
    public class RegexRouteConstraint : IRegexRouteConstraint, IRouteConstraint
    {
        public RegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) {
            Pattern = pattern;
            Options = options;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            var value = values[parameterName];
            if (value == null)
                return true;

            var valueAsString = value.ToString();

            return Regex.IsMatch(valueAsString, Pattern, Options);
        }

        /// <summary>
        ///  The regex pattern used to constrain the url parameter.
        ///  </summary>
        public string Pattern { get; private set; }

        /// <summary>
        ///  The RegexOptions used when testing the url parameter value
        ///  </summary>
        public RegexOptions Options { get; set; }
    }
}