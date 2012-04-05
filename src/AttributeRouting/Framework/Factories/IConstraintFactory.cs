using System.Text.RegularExpressions;

namespace AttributeRouting.Framework.Factories {
    public interface IConstraintFactory<out TConstraint> {

        TConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None);

        TConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods);
    }
}
