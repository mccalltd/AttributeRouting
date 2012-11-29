using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;
using System.Linq;

namespace AttributeRouting.Web.Constraints
{
    public class QueryStringRouteConstraintWrapper : IQueryStringRouteConstraintWrapper, IRouteConstraint
    {
        private readonly IRouteConstraint _constraint;

        public QueryStringRouteConstraintWrapper(IRouteConstraint constraint)
        {
            _constraint = constraint;
        }

        public object Constraint
        {
            get { return _constraint; }
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // If the query param does not exist, then fail.
            var queryString = httpContext.Request.QueryString;
            if (!queryString.AllKeys.Contains(parameterName))
                return false;

            // Process the constraint.
            var queryRouteValues = new RouteValueDictionary
            {
                { parameterName, queryString[parameterName] }
            };

            return _constraint.Match(httpContext, route, parameterName, queryRouteValues, routeDirection);
        }
    }
}