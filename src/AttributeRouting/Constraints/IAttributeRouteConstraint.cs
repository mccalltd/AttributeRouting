namespace AttributeRouting.Constraints {

    /// <summary>
    /// An Attribute Routing constraint which wraps an underlying constraint object (implemented by derived classes)
    /// </summary>
    public interface IAttributeRouteConstraint {
        /// <summary>
        /// The name of the route to apply this default against.
        /// </summary>
        string ForRouteNamed { get; set; }

        /// <summary>
        /// The key of the url parameter.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// The constraint to apply against url parameters with the specified key.
        /// </summary>
        object Constraint { get; }
    }
}
