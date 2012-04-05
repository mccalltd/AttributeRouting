using System.Text.RegularExpressions;
using System.Web.Http.Routing;

namespace AttributeRouting.WebApi
{
    /// <summary>
    /// Constrains a url parameter by a regex pattern.
    /// </summary>
    public class RegexRouteConstraintAttribute : RouteConstraintAttribute
    {
        /// <summary>
        /// Specify a regex constraint for a url parameter defined in a RouteAttribute applied to this action.
        /// </summary>
        /// <param name="key">The key of the url parameter</param>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        public RegexRouteConstraintAttribute(string key, string pattern)
            : this(key, pattern, RegexOptions.None) {}

        /// <summary>
        /// Specify a regex constraint for a url parameter defined in a RouteAttribute applied to this action.
        /// </summary>
        /// <param name="key">The key of the url parameter</param>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        /// <param name="options">The RegexOptions used when testing the url parameter value</param>
        public RegexRouteConstraintAttribute(string key, string pattern, RegexOptions options)
            : base(key)
        {
            Pattern = pattern;
            Options = options;
        }

        /// <summary>
        /// The regex pattern used to constrain the url parameter.
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// The RegexOptions used when testing the url parameter value
        /// </summary>
        public RegexOptions Options { get; set; }

        public override IHttpRouteConstraint Constraint
        {
            get { return new RegexRouteConstraint(Pattern, Options); }
        }
    }
}