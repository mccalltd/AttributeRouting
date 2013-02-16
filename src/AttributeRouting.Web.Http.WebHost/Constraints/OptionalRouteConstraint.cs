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
            // If the param is optional and has no value, then pass the constraint
            if (route.Defaults.ContainsKey(parameterName)
                && route.Defaults[parameterName] == RouteParameter.Optional)
            {
                if (values[parameterName].HasNoValue())
                    return true;
            }

            return _constraint.Match(httpContext, route, parameterName, values, routeDirection);
        }
    }
}