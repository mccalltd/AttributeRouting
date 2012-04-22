using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a value from an enum.
    /// </summary>
    public class EnumRouteConstraint<T> : EnumRouteConstraintBase<T>, IHttpRouteConstraint 
        where T : struct
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}
