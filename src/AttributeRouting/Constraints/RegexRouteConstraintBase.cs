using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constraints a url parameter by a regular expression.
    /// </summary>
    public abstract class RegexRouteConstraintBase : IAttributeRouteConstraint
    {
        protected RegexRouteConstraintBase(string pattern, RegexOptions options = RegexOptions.None)
        {
            Pattern = pattern;
            Options = options;
        }

        /// <summary>
        /// The pattern to match.
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// Regex options for matching.
        /// </summary>
        public RegexOptions Options { get; private set; }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeDictionary)
        {
            var value = routeDictionary[parameterName];
            if (value == null)
                return true;

            var valueAsString = value.ToString();

            return Regex.IsMatch(valueAsString, Pattern, Options);
        }
    }
}
