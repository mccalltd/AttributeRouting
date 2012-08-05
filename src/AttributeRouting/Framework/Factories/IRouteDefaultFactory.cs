namespace AttributeRouting.Framework.Factories
{
    /// <summary>
    /// Abstraction used by <see cref="RouteBuilder"/> 
    /// when generating route defaults.
    /// </summary>
    public interface IRouteDefaultFactory
    {
        /// <summary>
        /// Generates an optional parameter of the correct type.
        /// </summary>
        /// <remarks>
        /// Due to 
        /// UrlParameter.Optional (used in web-hosted scenarios) and
        /// RouteParameter.Optional (used in self-hosted scenarios).
        /// </remarks>
        object Optional();
    }
}
