namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategy for parsing subdomains from a host.
    /// </summary>
    public interface ISubdomainParser
    {
        string Execute(string host);
    }
}