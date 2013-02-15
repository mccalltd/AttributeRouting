using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class RegexHttpRouteConstraint : RegexRouteConstraintBase, IHttpRouteConstraint
    {
        public RegexHttpRouteConstraint(string pattern) : base(pattern) { }

        public RegexHttpRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) 
            : base(pattern, options) {}

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
