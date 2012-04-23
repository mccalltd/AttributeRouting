namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Wraps constraints to allow them to be optional.
    /// </summary>
    /// <remarks>
    /// Supports constraints specified like: param:constraint1?.
    /// </remarks>
    public interface IOptionalRouteConstraintWrapper
    {
        /// <summary>
        /// Constraint to consider optional.
        /// </summary>
        object Constraint { get; }
    }
}
