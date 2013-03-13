using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class BoolHttpRouteConstraint : TypeOfHttpRouteConstraint<bool> {}
    public class IntHttpRouteConstraint : TypeOfHttpRouteConstraint<int> {}
    public class LongHttpRouteConstraint : TypeOfHttpRouteConstraint<long> {}
    public class FloatHttpRouteConstraint : TypeOfHttpRouteConstraint<float> {}
    public class DoubleHttpRouteConstraint : TypeOfHttpRouteConstraint<double> {}
    public class DecimalHttpRouteConstraint : TypeOfHttpRouteConstraint<decimal> {}
    public class GuidHttpRouteConstraint : TypeOfHttpRouteConstraint<Guid> {}
    public class DateTimeHttpRouteConstraint : TypeOfHttpRouteConstraint<DateTime> {}

    public abstract class TypeOfHttpRouteConstraint<T> : TypeOfRouteConstraintBase<T>, IHttpRouteConstraint
        where T : struct
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(parameterName, values);
        }
    }
}