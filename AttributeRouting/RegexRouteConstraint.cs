using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting
{
    public class RegexRouteConstraint : IRouteConstraint 
    {
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