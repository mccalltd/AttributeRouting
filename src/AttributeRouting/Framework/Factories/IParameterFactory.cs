namespace AttributeRouting.Framework.Factories {

    /// <summary>
    /// Factory methods for getting RouteParameters or UrlParameters
    /// </summary>
    public interface IParameterFactory<out TRouteParameter> {

        /// <summary>
        /// Optional parameter
        /// </summary>
        /// <typeparam name="TRouteParameter"></typeparam>
        /// <returns></returns>
        TRouteParameter Optional();
    }
}
