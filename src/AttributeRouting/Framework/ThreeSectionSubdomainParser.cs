using System;
using System.Linq;
using System.Net;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategy that will parse all but the last two sections from a given host.
    /// </summary>
    public class ThreeSectionSubdomainParser : ISubdomainParser
    {
        public string Execute(string host)
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
        }
    }
}