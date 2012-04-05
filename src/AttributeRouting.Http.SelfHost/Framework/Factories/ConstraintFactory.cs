using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Common;
using System.Web.Http.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Http.SelfHost.Constraints;

namespace AttributeRouting.Http.SelfHost.Framework.Factories {
    public class ConstraintFactory : IConstraintFactory<IHttpRouteConstraint> {
        public IHttpRouteConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) {
            return new RegexRouteConstraint(pattern, options);
        }

        public IHttpRouteConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods) {
            return new RestfulHttpMethodConstraint(httpMethods.Select(HttpMethodHelper.GetHttpMethod).ToArray());
        }
    }
}