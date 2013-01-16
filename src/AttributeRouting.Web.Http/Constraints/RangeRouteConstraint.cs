using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class RangeRouteConstraint : RangeRouteConstraintBase, IHttpRouteConstraint
    {
        public RangeRouteConstraint(string min, string max)
        {
            MinConstraint = new MinRouteConstraint(min);
            MaxConstraint = new MaxRouteConstraint(max);
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}