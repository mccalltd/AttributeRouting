using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.SelfHost.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string of a length within a given range.
    /// </summary>
    public class RangeLengthRouteConstraint : RangeLengthRouteConstraintBase, IHttpRouteConstraint
    {
        public RangeLengthRouteConstraint(string minLength, string maxLength)
        {
            MinLengthConstraint = new MinLengthRouteConstraint(minLength);
            MaxLengthConstraint = new MaxLengthRouteConstraint(maxLength);
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}