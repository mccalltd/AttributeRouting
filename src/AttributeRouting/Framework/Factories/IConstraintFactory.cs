using System.Text.RegularExpressions;
using AttributeRouting.Constraints;

namespace AttributeRouting.Framework.Factories {

    /// <summary>
    /// A factory interface for generate AR constraints that implement framework interfaces (IRouteConstraint, IHttpRouteConstraint)
    /// </summary>
    public interface IConstraintFactory {

        /// <summary>
        /// Creates a new RegexRouteConstraint
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IRegexRouteConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None);

        /// <summary>
        /// Creates a new RestfulHttpMethodConstraint
        /// </summary>
        /// <param name="httpMethods"></param>
        /// <returns></returns>
        IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods);
    }
}
