namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategy for building route names from a route specification.
    /// </summary>
    interface IRouteNameBuilder
    {
        string Execute(RouteSpecification routeSpec);
    }
}