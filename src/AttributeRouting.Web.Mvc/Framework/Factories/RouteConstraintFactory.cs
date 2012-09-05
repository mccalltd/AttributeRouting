using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Constraints;
using AttributeRouting.Web.Mvc.Constraints;

namespace AttributeRouting.Web.Mvc.Framework.Factories
{
    internal class RouteConstraintFactory : IRouteConstraintFactory
    {
        private readonly AttributeRoutingConfiguration _configuration;

        public RouteConstraintFactory(AttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
        {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods)
        {
            return new RestfulHttpMethodConstraint(httpMethods);
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

        public ICompoundRouteConstraintWrapper CreateCompoundRouteConstraint(params object[] constraints)
        {
            return new CompoundRouteConstraintWrapper(constraints.Cast<IRouteConstraint>().ToArray());
        }

        public IOptionalRouteConstraintWrapper CreateOptionalRouteConstraint(object constraint)
        {
            return new OptionalRouteConstraintWrapper((IRouteConstraint)constraint);
        }
    }
}