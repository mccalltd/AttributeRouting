using System.Text.RegularExpressions;
using AttributeRouting.Constraints;

namespace AttributeRouting.Framework.Factories
{
    /// <summary>
    /// A factory interface for generate AR constraints that implement framework interfaces (IRouteConstraint, IHttpRouteConstraint)
    /// </summary>
    public interface IRouteConstraintFactory
    {
        /// <summary>
        /// Creates a new RegexRouteConstraint
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None);

        /// <summary>
        /// Creates a new RestfulHttpMethodConstraint
        /// </summary>
        /// <param name="httpMethods"></param>
        /// <returns></returns>
        IRestfulHttpMethodConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods);

        /// <summary>
        /// Creates an inline constraint of a specific type with the given parameters.
        /// </summary>
        /// <param name="name">The short name of the inline constraint</param>
        /// <param name="parameters">The paramters with which to construct the constraint</param>
        /// <returns></returns>
        object CreateInlineRouteConstraint(string name, params object[] parameters);

        /// <summary>
        /// Creates a compound route constraint to allow anding of individual inline constraints.
        /// </summary>
        /// <param name="constraints">The constraints to apply together</param>
        /// <returns></returns>
        ICompoundRouteConstraint CreateCompoundRouteConstraint(params object[] constraints);
    }
}