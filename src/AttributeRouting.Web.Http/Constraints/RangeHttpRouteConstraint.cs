using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class RangeHttpRouteConstraint : RangeRouteConstraintBase, IHttpRouteConstraint
    {
        public RangeHttpRouteConstraint(string min, string max)
        {
            MinConstraint = new MinHttpRouteConstraint(min);
            MaxConstraint = new MaxHttpRouteConstraint(max);
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}