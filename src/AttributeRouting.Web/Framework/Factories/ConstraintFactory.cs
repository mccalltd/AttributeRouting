using System.Text.RegularExpressions;
using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Framework.Factories {
    public class ConstraintFactory : IConstraintFactory {
        public IRegexRouteConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods) {
            return new RestfulHttpMethodConstraint(httpMethods);
        }
    }
}