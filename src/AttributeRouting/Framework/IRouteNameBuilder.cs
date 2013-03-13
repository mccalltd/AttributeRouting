namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategy for building route names from a route specification.
    /// </summary>
    public interface IRouteNameBuilder
    {
        string Execute(RouteSpecification routeSpec);
    }
}