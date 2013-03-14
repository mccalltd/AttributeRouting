using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class LengthHttpRouteConstraint : LengthRouteConstraintBase, IHttpRouteConstraint
    {
        public LengthHttpRouteConstraint(string length) : base(length) { }

        public LengthHttpRouteConstraint(string minLength, string maxLength)
            : base(new MinLengthHttpRouteConstraint(minLength),
                   new MaxLengthHttpRouteConstraint(maxLength)) { }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}