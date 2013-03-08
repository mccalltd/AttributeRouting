using System.Collections.Generic;
using System.Text.RegularExpressions;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter by a regular expression.
    /// </summary>
    public abstract class RegexRouteConstraintBase : IAttributeRouteConstraint
    {
        protected RegexRouteConstraintBase(string pattern)
            : this(pattern, RegexOptions.None)
        { }

        protected RegexRouteConstraintBase(string pattern, RegexOptions options)
        {
            Pattern = pattern;
            // REVIEW: Shouldn't these be included in the derived classes by default: RegexOptions.CultureInvariant | RegexOptions.IgnoreCase?
            Options = options;  // No need to tell user that it is 'compiled' option...so do not include in public options
            CompiledExpression = new Regex(pattern, options | RegexOptions.Compiled); 
        }

        /// <summary>
        /// The pattern to match.
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// The compiled reg-ex expression.
        /// </summary>
        private Regex CompiledExpression { get; set; }

        /// <summary>
        /// Regex options for matching.
        /// </summary>
        public RegexOptions Options { get; private set; }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeDictionary)
        {
            var value = routeDictionary[parameterName];
            if (value.HasNoValue())
                return true;

            var valueAsString = value.ToString();
            return CompiledExpression.IsMatch(valueAsString);
        }
    }
}
