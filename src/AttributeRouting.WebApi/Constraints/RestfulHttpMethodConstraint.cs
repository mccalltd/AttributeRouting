using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Common;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.WebApi
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    public class RestfulHttpMethodConstraint : RestfulHttpMethodConstraintBase, IHttpRouteConstraint
    {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        public RestfulHttpMethodConstraint(params HttpMethod[] allowedMethods)
            : base(allowedMethods.Select(method => method.Method).ToArray()) { }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return IsMatch(routeDirection == HttpRouteDirection.UriGeneration, request.Method.Method);
        }
    }
}