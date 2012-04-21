using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.SelfHost.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a long with a maximum value.
    /// </summary>
    public class MaxRouteConstraint : MaxRouteConstraintBase, IHttpRouteConstraint
    {
        public MaxRouteConstraint(string max) : base (max) {}

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}