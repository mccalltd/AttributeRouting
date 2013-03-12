using System.Web;
using System.Web.Http;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.WebHost.Constraints
{
    public class OptionalRouteConstraint : IOptionalRouteConstraint, IRouteConstraint
    {
        private readonly IRouteConstraint _constraint;

        public OptionalRouteConstraint(IRouteConstraint constraint)
        {
            _constraint = constraint;
        }

        public object Constraint
        {
            get { return _constraint; }
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // NOTE: Due to webhost hijacking the route added to the route collection, this route is NOT an IAttributeRoute.
            // As such we cannot check the querystring defaults as we do in the other impls of IOptionalRouteConstraint.
            var allDefaults = route.Defaults;

            // If the param is optional and has no value, then pass the constraint
            if (allDefaults.ContainsKey(parameterName) && allDefaults[parameterName] == RouteParameter.Optional)
            {
                if (values[parameterName].HasNoValue())
                {
                    return true;
                }
            }

            return _constraint.Match(httpContext, route, parameterName, values, routeDirection);
        }
    }
}