using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Constraints
{
    public class QueryStringRouteConstraint : IQueryStringRouteConstraint, IRouteConstraint
    {
        private readonly IRouteConstraint _constraint;

        public QueryStringRouteConstraint(IRouteConstraint constraint)
        {
            _constraint = constraint;
        }

        public object Constraint
        {
            get { return _constraint; }
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // If the query param does not exist in the query or the route defaults, then fail.
            var queryString = httpContext.Request.QueryString;
            var value = queryString[parameterName] ?? values[parameterName];
            if (value.HasNoValue())
            {
                return false;
            }

            // Process the constraint.
            var queryRouteValues = new RouteValueDictionary
            {
                { parameterName, value }
            };

            return _constraint == null // ie: Simply ensure that the query param exists.
                   || _constraint.Match(httpContext, route, parameterName, queryRouteValues, routeDirection);
        }
    }
}