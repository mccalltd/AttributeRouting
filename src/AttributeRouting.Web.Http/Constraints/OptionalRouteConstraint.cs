using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.Constraints
{
    public class OptionalRouteConstraint : IOptionalRouteConstraint, IHttpRouteConstraint
    {
        private readonly IHttpRouteConstraint _constraint;

        public OptionalRouteConstraint(IHttpRouteConstraint constraint)
        {
            _constraint = constraint;
        }

        public object Constraint
        {
            get { return _constraint; }
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            // If the param is optional and has no value, then pass the constraint
            if (route.Defaults.ContainsKey(parameterName)
                && route.Defaults[parameterName] == RouteParameter.Optional)
            {
                if (values[parameterName].HasNoValue())
                    return true;
            }

            return _constraint.Match(request, route, parameterName, values, routeDirection);
        }
    }
}