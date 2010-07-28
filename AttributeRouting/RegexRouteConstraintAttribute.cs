using System.Text.RegularExpressions;
using System.Web.Routing;

namespace AttributeRouting
{
    public class RegexRouteConstraintAttribute : RouteConstraintAttribute
    {
        public RegexRouteConstraintAttribute(string key, string pattern) 
            : this(key, pattern, RegexOptions.None) {}

        public RegexRouteConstraintAttribute(string key, string pattern, RegexOptions options) 
            : base(key)
        {
            Pattern = pattern;
            Options = options;
        }

        public string Pattern { get; private set; }

        public RegexOptions Options { get; set; }

        public override IRouteConstraint Constraint
        {
            get { return new RegexRouteConstraint(Pattern, Options); }
        }
    }
}
