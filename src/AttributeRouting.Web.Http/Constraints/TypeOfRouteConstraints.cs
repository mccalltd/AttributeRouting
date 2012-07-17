using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class BoolRouteConstraint : TypeOfRouteConstraint<bool> {}
    public class IntRouteConstraint : TypeOfRouteConstraint<int> {}
    public class LongRouteConstraint : TypeOfRouteConstraint<long> {}
    public class FloatRouteConstraint : TypeOfRouteConstraint<float> {}
    public class DoubleRouteConstraint : TypeOfRouteConstraint<double> {}
    public class DecimalRouteConstraint : TypeOfRouteConstraint<decimal> {}
    public class GuidRouteConstraint : TypeOfRouteConstraint<Guid> {}
    public class DateTimeRouteConstraint : TypeOfRouteConstraint<DateTime> {}

    public abstract class TypeOfRouteConstraint<T> : TypeOfRouteConstraintBase<T>, IHttpRouteConstraint
        where T : struct
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}