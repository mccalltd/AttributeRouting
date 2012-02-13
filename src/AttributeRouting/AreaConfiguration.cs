using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting
{
    /// <summary>
    /// Helper for configuring areas when initializing AttributeRouting framework.
    /// </summary>
    public class AreaConfiguration
    {
        private readonly string _name;
        private readonly AttributeRoutingConfiguration _configuration;

        /// <summary>
        /// Helper for configuring areas when initializing AttributeRouting framework.
        /// </summary>
        public AreaConfiguration(string name, AttributeRoutingConfiguration configuration)
        {
            _name = name;
            _configuration = configuration;
        }

        /// <summary>
        /// Set the subdomain this area is mapped to.
        /// </summary>
        /// <param name="subdomain">The name fo the subdomain</param>
        public void ToSubdomain(string subdomain)
        {
            if (_configuration.AreaSubdomainOverrides.ContainsKey(_name))
                throw new AttributeRoutingException(
                    "The area \"{0}\" is already mapped to a subdomain.".FormatWith(_name));

            _configuration.AreaSubdomainOverrides.Add(_name, subdomain);
        }
    }
}