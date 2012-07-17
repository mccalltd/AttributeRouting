namespace AttributeRouting.Constraints
{
    public abstract class AlphaRouteConstraintBase : RegexRouteConstraintBase
    {
        protected AlphaRouteConstraintBase() : base(@"^[A-Za-z]*$") {}
    }
}