using System;
using System.Linq;

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
                var sections = host.Split('.');
                return sections.Length < 3
                           ? null
                           : String.Join(".", sections.Take(sections.Length - 2));
            };
        }
    }
}