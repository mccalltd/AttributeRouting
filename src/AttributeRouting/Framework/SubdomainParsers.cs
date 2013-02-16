using System;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategies for parsing the subdomain from host names.
    /// </summary>
    /// <remarks>
    /// A bit awkward currently due to initial impl not being formalized into separate strategy classes.
    /// Will refactor use via configuration API for v4.0.
    /// </remarks>
    public static class SubdomainParsers
    {
        /// <summary>
        /// Will parse all but the last two sections from a given host.
        /// </summary>
        public static Func<string, string> ThreeSection
        {
            get { return new ThreeSectionSubdomainParser().Execute; }
        } 
    }
}