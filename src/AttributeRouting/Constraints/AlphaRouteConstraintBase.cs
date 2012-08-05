namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constrains a url parameter to contain only letters from the alphabet.
    /// </summary>
    public abstract class AlphaRouteConstraintBase : RegexRouteConstraintBase
    {
        protected AlphaRouteConstraintBase() : base(@"^[A-Za-z]*$") {}
    }
}