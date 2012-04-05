using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AttributeRouting {

    /// <summary>
    /// Applies a regex constraint against the associated url parameter.
    /// </summary>
    public abstract class RegexRouteConstraintBase {

        /// <summary>
        /// Applies a regex constraint against the associated url parameter.
        /// </summary>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        public RegexRouteConstraintBase(string pattern) : this(pattern, RegexOptions.None) {}

        /// <summary>
        /// Applies a regex constraint against the associated url parameter.
        /// </summary>
        /// <param name="pattern">The regex pattern used to constrain the url parameter</param>
        /// <param name="options">The RegexOptions used when testing the url parameter value</param>
        public RegexRouteConstraintBase(string pattern, RegexOptions options)
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
    }
}
