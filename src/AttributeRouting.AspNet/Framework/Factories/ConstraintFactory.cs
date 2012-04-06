using System.Text.RegularExpressions;
using System.Web.Routing;
using AttributeRouting.Framework.Factories;
using HttpMethodConstraint = AttributeRouting.Web.HttpMethodConstraint;

namespace AttributeRouting.Web.Framework.Factories {
    public class ConstraintFactory : IConstraintFactory<IRouteConstraint> {
        public IRouteConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRouteConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods) {
            return new HttpMethodConstraint(httpMethods);
        }
    }
}