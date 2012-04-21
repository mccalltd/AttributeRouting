using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.SelfHost.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string with a maximum length.
    /// </summary>
    public class MinLengthRouteConstraint : MinLengthRouteConstraintBase, IHttpRouteConstraint
    {
        public MinLengthRouteConstraint(string minLength) : base(minLength) { }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}