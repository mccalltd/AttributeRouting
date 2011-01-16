using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttributeRouting
{
    public class RegexRouteConstraint : IRouteConstraint 
    {
        /// <summary>
        /// Applies a regex constraint against the associated url parameter.
        /// </summary>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        public RegexRouteConstraint(string pattern) : this(pattern, RegexOptions.None) { }

        /// <summary>
        /// Applies a regex constraint against the associated url parameter.
        /// </summary>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        /// <param name="options">The RegexOptions to use when testing the url parameter value</param>
        public RegexRouteConstraint(string pattern, RegexOptions options)
        {
            Pattern = pattern;
            Options = options;
        }

        public string Pattern { get; private set; }

        public RegexOptions Options { get; private set; }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[parameterName];
            if (value == null)
                return true;

            var valueAsString = value.ToString();

            return Regex.IsMatch(valueAsString, Pattern, Options);
        }
    }
}