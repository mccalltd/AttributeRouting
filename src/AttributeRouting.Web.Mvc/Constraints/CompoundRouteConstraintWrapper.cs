using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Mvc.Constraints
{
    public class CompoundRouteConstraintWrapper : ICompoundRouteConstraintWrapper, IRouteConstraint
    {
        private readonly IRouteConstraint[] _constraints;

        public CompoundRouteConstraintWrapper(params IRouteConstraint[] constraints)
        {
            _constraints = constraints;
        }

        public IEnumerable<object> Constraints
        {
            get { return _constraints; }
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _constraints.All(c => c.Match(httpContext, route, parameterName, values, routeDirection));
        }
    }
}
