namespace AttributeRouting.Framework.Factories
{
    /// <summary>
    /// Factory methods for getting RouteParameters or UrlParameters
    /// </summary>
    public interface IParameterFactory
    {
        /// <summary>
        /// Optional parameter (UrlParameter.Optional, RouteParameter.Optional)
        /// </summary>
        /// <returns></returns>
        object Optional();
    }
}
