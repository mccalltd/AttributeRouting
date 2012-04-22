using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string of a length within a given range.
    /// </summary>
    public class LengthRouteConstraint : LengthRouteConstraintBase, IHttpRouteConstraint
    {
        public LengthRouteConstraint(string length) : base(length) { }

        public LengthRouteConstraint(string minLength, string maxLength)
            : base(new MinLengthRouteConstraint(minLength),
                   new MaxLengthRouteConstraint(maxLength)) { }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}