namespace AttributeRouting.Web.Constraints
{
    public class AlphaRouteConstraint : RegexRouteConstraint
    {
        public AlphaRouteConstraint() : base(@"^[A-Za-z]*$") {}
    }
}
