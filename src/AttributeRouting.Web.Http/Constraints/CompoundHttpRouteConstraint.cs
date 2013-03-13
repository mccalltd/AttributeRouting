using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class CompoundHttpRouteConstraint : ICompoundRouteConstraint, IHttpRouteConstraint
    {
        private readonly IHttpRouteConstraint[] _constraints;

        public CompoundHttpRouteConstraint(params IHttpRouteConstraint[] constraints)
        {
            _constraints = constraints;
        }

        public IEnumerable<object> Constraints
        {
            get { return _constraints; }
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return _constraints.All(c => c.Match(request, route, parameterName, values, routeDirection));
        }
    }
}
