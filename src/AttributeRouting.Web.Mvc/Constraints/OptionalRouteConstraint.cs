using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
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
            var attributeRoute = (IAttributeRoute)route;
            var allDefaults = new Dictionary<string, object>();
            allDefaults.Merge(attributeRoute.Defaults);
            allDefaults.Merge(attributeRoute.QueryStringDefaults);
            
            // If the param is optional and has no value, then pass the constraint
            if (allDefaults.ContainsKey(parameterName) && allDefaults[parameterName] == UrlParameter.Optional)
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