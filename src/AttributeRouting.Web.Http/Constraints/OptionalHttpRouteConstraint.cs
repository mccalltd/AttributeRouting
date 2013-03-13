using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.Constraints
{
    public class OptionalHttpRouteConstraint : IOptionalRouteConstraint, IHttpRouteConstraint
    {
        private readonly IHttpRouteConstraint _constraint;

        public OptionalHttpRouteConstraint(IHttpRouteConstraint constraint)
        {
            _constraint = constraint;
        }

        public object Constraint
        {
            get { return _constraint; }
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            var attributeRoute = (IAttributeRoute)route;
            var allDefaults = new Dictionary<string, object>();
            allDefaults.Merge(attributeRoute.Defaults);
            allDefaults.Merge(attributeRoute.QueryStringDefaults);

            // If the param is optional and has no value, then pass the constraint
            if (allDefaults.ContainsKey(parameterName) && allDefaults[parameterName] == RouteParameter.Optional)
            {
                if (values[parameterName].HasNoValue())
                {
                    return true;
                }
            }

            return _constraint.Match(request, route, parameterName, values, routeDirection);
        }
    }
}