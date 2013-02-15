using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http.Constraints;

namespace AttributeRouting.Web.Http.Framework.Factories
{
    internal class RouteConstraintFactory : IRouteConstraintFactory
    {
        private readonly HttpConfiguration _configuration;

        public RouteConstraintFactory(HttpConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
        {
            return new RegexHttpRouteConstraint(pattern, options);
        }

        public IInboundHttpMethodConstraint CreateInboundHttpMethodConstraint(string[] httpMethods)
        {
            return new InboundHttpMethodConstraint(httpMethods.Select(m => new HttpMethod(m)).ToArray());
        }

        public object CreateInlineRouteConstraint(string name, params object[] parameters)
        {
            var inlineRouteConstraints = _configuration.InlineRouteConstraints;
            if (inlineRouteConstraints.ContainsKey(name))
            {
                var type = inlineRouteConstraints[name];

                if (!typeof(IHttpRouteConstraint).IsAssignableFrom(type))
                {
                    throw new AttributeRoutingException(
                        "The constraint \"{0}\" must implement System.Web.Routing.IHttpRouteConstraint".FormatWith(type.FullName));
                }

                return Activator.CreateInstance(type, parameters);
            }

            return null;
        }

        public ICompoundRouteConstraintWrapper CreateCompoundRouteConstraint(params object[] constraints)
        {
            return new CompoundHttpRouteConstraintWrapper(constraints.Cast<IHttpRouteConstraint>().ToArray());
        }

        public IOptionalRouteConstraintWrapper CreateOptionalRouteConstraint(object constraint)
        {
            return new OptionalHttpRouteConstraintWrapper((IHttpRouteConstraint)constraint);
        }

        public IQueryStringRouteConstraintWrapper CreateQueryStringRouteConstraint(object constraint)
        {
            return new QueryStringHttpRouteConstraintWrapper((IHttpRouteConstraint)constraint);
        }
    }
}