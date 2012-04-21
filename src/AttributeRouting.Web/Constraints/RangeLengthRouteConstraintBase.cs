using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Constraints
{
    /// <summary>
    /// Constraints a url parameter to be a string of a length within a given range.
    /// </summary>
    public class RangeLengthRouteConstraint : RangeLengthRouteConstraintBase, IRouteConstraint
    {
        public RangeLengthRouteConstraint(string minLength, string maxLength)
        {
            MinLengthConstraint = new MinLengthRouteConstraint(minLength);
            MaxLengthConstraint = new MaxLengthRouteConstraint(maxLength);
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}