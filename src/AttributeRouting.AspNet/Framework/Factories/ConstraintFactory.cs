using System.Text.RegularExpressions;
using System.Web.Routing;
using AttributeRouting.AspNet.Constraints;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.AspNet.Framework.Factories {
    public class ConstraintFactory : IConstraintFactory<IRouteConstraint> {
        public IRouteConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRouteConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods) {
            return new RestfulHttpMethodConstraint(httpMethods);
        }
    }
}