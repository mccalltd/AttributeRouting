using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// A regular expression to constrain the route against. This is the base class containing shared logic.
    /// </summary>
    public abstract class RegexRouteConstraintBase
    {
        protected RegexRouteConstraintBase(string pattern, RegexOptions options = RegexOptions.None)
        {
            Pattern = pattern;
            Options = options;
        }

        public string Pattern { get; private set; }

        public RegexOptions Options { get; set; }

        protected bool IsMatch(string parameterName, IDictionary<string, object> routeDictionary)
        {
            var value = routeDictionary[parameterName];
            if (value == null)
                return true;

            var valueAsString = value.ToString();

            return Regex.IsMatch(valueAsString, Pattern, Options);
        }
    }
}
