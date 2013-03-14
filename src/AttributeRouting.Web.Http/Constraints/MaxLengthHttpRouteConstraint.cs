using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class MaxLengthHttpRouteConstraint : MaxLengthRouteConstraintBase, IHttpRouteConstraint
    {
        public MaxLengthHttpRouteConstraint(string maxLength) : base(maxLength) { }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
