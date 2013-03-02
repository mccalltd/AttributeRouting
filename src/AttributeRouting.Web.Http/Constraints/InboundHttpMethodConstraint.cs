using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.Constraints
{
    public class InboundHttpMethodConstraint : HttpMethodConstraint, IInboundHttpMethodConstraint
    {
        /// <summary>
        /// Constrains an inbound route by HTTP method.
        /// </summary>
        public InboundHttpMethodConstraint(params HttpMethod[] allowedMethods)
            : base(allowedMethods)
        {
        }

        ICollection<string> IInboundHttpMethodConstraint.AllowedMethods
        {
            get { return new ReadOnlyCollection<string>(AllowedMethods.Select(method => method.Method).ToList()); }
        }

        protected override bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriGeneration)
                return true;

            return base.Match(request, route, parameterName, values, routeDirection);
        }
    }
}