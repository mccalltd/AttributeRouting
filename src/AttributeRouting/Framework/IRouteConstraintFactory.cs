using System.Text.RegularExpressions;
using AttributeRouting.Constraints;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Abstraction used by <see cref="RouteBuilder"/> when generating a route constraint.
    /// </summary>
    /// <remarks>
    /// Due to 
    /// System.Web.Routing.IRouteConstraint (used in web-hosted scenarios) and 
    /// System.Web.Http.Routing.IHttpRouteConstraint (used in self-hosted scenarios).    
    /// </remarks>
    public interface IRouteConstraintFactory
    {
        /// <summary>
        /// Creates a new regex route constraint.
        /// </summary>
        RegexRouteConstraintBase CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None);

        /// <summary>
        /// Creates a new inbound http method constraint.
        /// </summary>
        IInboundHttpMethodConstraint CreateInboundHttpMethodConstraint(string[] httpMethods);

        /// <summary>
        /// Creates an inline constraint of a specific type with the given parameters.
        /// </summary>
        /// <param name="name">The short name of the inline constraint</param>
        /// <param name="parameters">The paramters with which to construct the constraint</param>
        object CreateInlineRouteConstraint(string name, params object[] parameters);

        /// <summary>
        /// Creates a compound route constraint wrapper to allow anding of individual inline constraints.
        /// </summary>
        /// <param name="constraints">The constraints to apply together</param>
        ICompoundRouteConstraint CreateCompoundRouteConstraint(params object[] constraints);

        /// <summary>
        /// Creates an optional route constraint wrapper to allow inline constraints to be optional.
        /// </summary>
        /// <param name="constraint">The constraint</param>
        IOptionalRouteConstraint CreateOptionalRouteConstraint(object constraint);

        /// <summary>
        /// Creates an query string route constraint wrapper to allow constraints in the query string.
        /// </summary>
        /// <param name="constraint">The constraint</param>
        IQueryStringRouteConstraint CreateQueryStringRouteConstraint(object constraint);
    }
}