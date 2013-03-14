using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc.Constraints;

namespace AttributeRouting.Web.Mvc.Framework
{
    internal class RouteConstraintFactory : IRouteConstraintFactory
    {
        private readonly RouteConfiguration _configuration;

        public RouteConstraintFactory(RouteConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
        {
            return new RegexRouteConstraint(pattern, options);
        }

        public IInboundHttpMethodConstraint CreateInboundHttpMethodConstraint(string[] httpMethods)
        {
            return new InboundHttpMethodConstraint(httpMethods);
        }

        public object CreateInlineRouteConstraint(string name, params object[] parameters)
        {
            var inlineRouteConstraints = _configuration.InlineRouteConstraints;
            if (inlineRouteConstraints.ContainsKey(name))
            {
                var type = inlineRouteConstraints[name];

                if (!typeof(IRouteConstraint).IsAssignableFrom(type))
                    throw new AttributeRoutingException(
                        "The constraint \"{0}\" must implement System.Web.Routing.IRouteConstraint".FormatWith(type.FullName));

                return Activator.CreateInstance(type, parameters);
            }

            return null;
        }

        public ICompoundRouteConstraint CreateCompoundRouteConstraint(params object[] constraints)
        {
            return new CompoundRouteConstraint(constraints.Cast<IRouteConstraint>().ToArray());
        }

        public IOptionalRouteConstraint CreateOptionalRouteConstraint(object constraint)
        {
            return new OptionalRouteConstraint((IRouteConstraint)constraint);
        }

        public IQueryStringRouteConstraint CreateQueryStringRouteConstraint(object constraint)
        {
            return new QueryStringRouteConstraint((IRouteConstraint)constraint);
        }
    }
}