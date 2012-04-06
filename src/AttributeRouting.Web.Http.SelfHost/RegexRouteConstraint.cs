using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.SelfHost
{
    /// <summary>
    /// Applies a regex constraint against the associated url parameter.
    /// </summary>
    public class RegexRouteConstraint : RegexRouteConstraintBase, IHttpRouteConstraint
    {
        public RegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) : base(pattern, options)
        {
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return base.IsMatch(parameterName, values);
        }
    }
}