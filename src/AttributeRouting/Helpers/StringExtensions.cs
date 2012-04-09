using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AttributeRouting.Helpers
{
    public static class StringExtensions
    {
        public static bool ValueEquals(this string s, string other)
        {
            if (s == null)
                return other == null;

            return s.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasValue(this string s)
        {
            return !String.IsNullOrWhiteSpace(s);
        }

        public static bool HasNoValue(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        public static string FormatWith(this string s, params object[] args)
        {
            return String.Format(s, args);
        }

        public static string[] SplitAndTrim(this string s, string[] separator)
        {
            if (!s.HasValue())
                return null;

            return s.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray();
        }

        public static bool IsValidUrl(this string s, bool allowTokens = false)
        {
            var urlParts = s.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            var invalidUrlPatterns = new List<string>
            {
                @"[#%&:<>/{0}]".FormatWith(allowTokens ? null : @"\\\+\{\}?\*"),
                @"\.\.",
                @"\.$",
                @"^ ",
                @" $"
            };

            var invalidUrlPattern = String.Join("|", invalidUrlPatterns);

            return !urlParts.Any(p => Regex.IsMatch(p, invalidUrlPattern));
        }
    }
}
