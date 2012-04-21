using System.Web;
using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Constraints
{
    public class BoolRouteConstraint : TypeOfRouteConstraint<bool> {}
    public class IntRouteConstraint : TypeOfRouteConstraint<int> {}
    public class LongRouteConstraint : TypeOfRouteConstraint<long> {}
    public class FloatRouteConstraint : TypeOfRouteConstraint<float> {}
    public class DoubleRouteConstraint : TypeOfRouteConstraint<double> {}
    public class DecimalRouteConstraint : TypeOfRouteConstraint<decimal> {}

    public abstract class TypeOfRouteConstraint<T> : TypeOfRouteConstraintBase<T>, IRouteConstraint
        where T : struct
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}