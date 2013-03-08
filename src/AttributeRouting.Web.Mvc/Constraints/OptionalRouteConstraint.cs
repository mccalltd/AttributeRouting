using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Mvc.Constraints
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
                && route.Defaults[parameterName] == UrlParameter.Optional)
            {
                var value = values[parameterName];
                if (value == UrlParameter.Optional || value.HasNoValue())
                    return true;
            }

            return _constraint.Match(httpContext, route, parameterName, values, routeDirection);
        }
    }
}