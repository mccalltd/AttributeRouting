namespace AttributeRouting.Web.Http.Constraints
{
    public class AlphaRouteConstraint : RegexRouteConstraint
    {
        public AlphaRouteConstraint() : base(@"^[A-Za-z]*$") {}
    }
}
