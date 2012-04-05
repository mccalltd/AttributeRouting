namespace AttributeRouting.Constraints {
    public interface IRouteConstraint<out TConstraint> {
        /// <summary>
        /// The name of the route to apply this default against.
        /// </summary>
        string ForRouteNamed { get; set; }

        /// <summary>
        /// The key of the url parameter.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// The TConstraint to apply against url parameters with the specified key.
        /// </summary>
        TConstraint Constraint { get; }
    }
}
