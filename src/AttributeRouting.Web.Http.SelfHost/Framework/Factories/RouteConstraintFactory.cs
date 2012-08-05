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
using AttributeRouting.Web.Http.SelfHost.Constraints;
using OptionalRouteConstraintWrapper = AttributeRouting.Web.Http.SelfHost.Constraints.OptionalRouteConstraintWrapper;

namespace AttributeRouting.Web.Http.SelfHost.Framework.Factories
{
    public class RouteConstraintFactory : IRouteConstraintFactory
    {
        private readonly HttpAttributeRoutingConfiguration _configuration;

        public RouteConstraintFactory(HttpAttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
        {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods)
        {
            var allowedMethods = httpMethods.Select(m => new HttpMethod(m)).ToArray();
            return new RestfulHttpMethodConstraint(allowedMethods);
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

        public ICompoundRouteConstraintWrapper CreateCompoundRouteConstraint(params object[] constraints)
        {
            return new CompoundRouteConstraintWrapper(constraints.Cast<IHttpRouteConstraint>().ToArray());
        }

        public IOptionalRouteConstraintWrapper CreateOptionalRouteConstraint(object constraint)
        {
            return new OptionalRouteConstraintWrapper((IHttpRouteConstraint)constraint);
        }
    }
}