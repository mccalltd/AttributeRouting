using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Common;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http.Constraints;

namespace AttributeRouting.Web.Http.SelfHost.Framework.Factories
{
    public class HttpRouteConstraintFactory : IRouteConstraintFactory
    {
        private readonly HttpAttributeRoutingConfiguration _configuration;

        public HttpRouteConstraintFactory(HttpAttributeRoutingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
        {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods)
        {
            return new RestfulHttpMethodConstraint(httpMethods.Select(HttpMethodHelper.GetHttpMethod).ToArray());
        }

        public object CreateInlineRouteConstraint(string name, params object[] parameters)
        {
            var inlineRouteConstraints = _configuration.InlineRouteConstraints;
            if (inlineRouteConstraints.ContainsKey(name))
            {
                var type = inlineRouteConstraints[name];

                if (!typeof(IHttpRouteConstraint).IsAssignableFrom(type))
                    throw new AttributeRoutingException(
                        "The constraint \"{0}\" must implement System.Web.Routing.IRouteConstraint".FormatWith(type.FullName));

                return Activator.CreateInstance(type, parameters) as IAttributeRouteConstraint;
            }

            return null;
        }

        public ICompoundRouteConstraint CreateCompoundRouteConstraint(params object[] constraints)
        {
            return new CompoundRouteConstraint(constraints.Cast<IHttpRouteConstraint>().ToArray());
        }
    }
}