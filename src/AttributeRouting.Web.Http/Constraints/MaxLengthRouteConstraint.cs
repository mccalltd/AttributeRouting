using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class MaxLengthRouteConstraint : MaxLengthRouteConstraintBase, IHttpRouteConstraint
    {
        public MaxLengthRouteConstraint(string maxLength) : base(maxLength) { }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
