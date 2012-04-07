using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Common;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost.Framework.Factories {
    public class HttpRouteConstraintFactory : IConstraintFactory {
        public IRegexRouteConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None) {
            return new RegexRouteConstraint(pattern, options);
        }

        public IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods) {
            return new RestfulHttpMethodConstraint(httpMethods.Select(HttpMethodHelper.GetHttpMethod).ToArray());
        }
    }
}