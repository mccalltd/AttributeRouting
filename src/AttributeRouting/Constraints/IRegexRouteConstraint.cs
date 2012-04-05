using System.Text.RegularExpressions;

namespace AttributeRouting.Constraints {
    /// <summary>
    /// Applies a regex constraint against the associated url parameter.
    /// </summary>
    public interface IRegexRouteConstraint {
        /// <summary>
        ///  The regex pattern used to constrain the url parameter.
        ///  </summary>
        string Pattern { get; }

        /// <summary>
        ///  The RegexOptions used when testing the url parameter value
        ///  </summary>
        RegexOptions Options { get; set; }
    }
}
