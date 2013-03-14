using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http.Constraints;

namespace AttributeRouting.Web.Http.Framework
{
    public class RouteConstraintFactory : IRouteConstraintFactory
    {
        private readonly HttpRouteConfigurationBase _configuration;

        public RouteConstraintFactory(HttpRouteConfigurationBase configuration)
        {
            _configuration = configuration;
        }

        public RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
        {
            return new RegexHttpRouteConstraint(pattern, options);
        }

        public IInboundHttpMethodConstraint CreateInboundHttpMethodConstraint(string[] httpMethods)
        {
            var allowedMethods = httpMethods.Select(m => new HttpMethod(m)).ToArray();
            return new InboundHttpMethodConstraint(allowedMethods);
        }

        public object CreateInlineRouteConstraint(string name, params object[] parameters)
        {
            var inlineRouteConstraints = _configuration.InlineRouteConstraints;
            if (inlineRouteConstraints.ContainsKey(name))
            {
                var type = inlineRouteConstraints[name];

                if (!typeof(IHttpRouteConstraint).IsAssignableFrom(type))
                    throw new AttributeRoutingException(
                        "The constraint \"{0}\" must implement System.Web.Http.Routing.IHttpRouteConstraint".FormatWith(type.FullName));

                return Activator.CreateInstance(type, parameters);
            }

            return null;
        }

        public ICompoundRouteConstraint CreateCompoundRouteConstraint(params object[] constraints)
        {
            return new CompoundHttpRouteConstraint(constraints.Cast<IHttpRouteConstraint>().ToArray());
        }

        public IOptionalRouteConstraint CreateOptionalRouteConstraint(object constraint)
        {
            return new OptionalHttpRouteConstraint((IHttpRouteConstraint)constraint);
        }

        public IQueryStringRouteConstraint CreateQueryStringRouteConstraint(object constraint)
        {
            return new QueryStringHttpRouteConstraint((IHttpRouteConstraint)constraint);
        }
    }
}