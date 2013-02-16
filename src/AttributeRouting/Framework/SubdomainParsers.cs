using System;
using System.Linq;
using System.Net;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Subdomain parsers for parsing the subdomain from host names.
    /// </summary>
    public static class SubdomainParsers
    {
        /// <summary>
        /// This parser pulls the first part of a three-section hostanme as the subdomain.
        /// </summary>
        public static Func<string, string> ThreeSection
        {
            get { return CreateThreeSectionSubdomainParser(); }
        } 

        private static Func<string, string> CreateThreeSectionSubdomainParser()
        {
            return host =>
            {
                // If host has no value return it.
                if (host.HasNoValue())
                {
                    return null;
                }

                // Don't include the port in the parsing logic.
                if (host.Contains(":"))
                {
                    host = host.Substring(0, host.IndexOf(":", StringComparison.Ordinal));
                }

                // Test for an ip since there's no subdomain to parse.
                IPAddress ip;
                if (IPAddress.TryParse(host, out ip))
                {
                    return null;
                }
                
                // Split the sections and take all but the last two.
                var sections = host.Split('.');
                return sections.Length < 3
                           ? null
                           : String.Join(".", sections.Take(sections.Length - 2));
            };
        }
    }
}