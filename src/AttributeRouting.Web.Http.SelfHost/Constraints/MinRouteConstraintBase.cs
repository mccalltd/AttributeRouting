using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.SelfHost.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a long with a minimum value.
    /// </summary>
    public class MinRouteConstraint : MinRouteConstraintBase, IHttpRouteConstraint
    {
        public MinRouteConstraint(string min) : base(min) {}

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}