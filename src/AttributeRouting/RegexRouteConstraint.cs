using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting
{
    /// <summary>
    /// Applies a regex constraint against the associated url parameter.
    /// </summary>
    public class RegexRouteConstraint : IRouteConstraint
    {
        /// <summary>
        /// Applies a regex constraint against the associated url parameter.
        /// </summary>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        public RegexRouteConstraint(string pattern) : this(pattern, RegexOptions.None) {}

        /// <summary>
        /// Applies a regex constraint against the associated url parameter.
        /// </summary>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        /// <param name="options">The RegexOptions used when testing the url parameter value</param>
        public RegexRouteConstraint(string pattern, RegexOptions options)
        {
            Pattern = pattern;
            Options = options;
        }

        /// <summary>
        /// The regex pattern used to constrain the url parameter.
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// The RegexOptions used when testing the url parameter value
        /// </summary>
        public RegexOptions Options { get; set; }

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