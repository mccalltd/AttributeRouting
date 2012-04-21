using System.Text.RegularExpressions;
using AttributeRouting.Constraints;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Constraints;

namespace AttributeRouting.Web.Framework.Factories
{
    public class ConstraintFactory : IConstraintFactory
    {
        public RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None)
        {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods)
        {
            return new RestfulHttpMethodConstraint(httpMethods);
        }
    }
}