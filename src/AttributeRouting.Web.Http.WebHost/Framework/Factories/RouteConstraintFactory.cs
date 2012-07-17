using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Http.WebHost.Constraints;

namespace AttributeRouting.Web.Http.WebHost.Framework.Factories
{
    public class RouteConstraintFactory : WebRouteConstraintFactoryBase
    {
        public RouteConstraintFactory(HttpAttributeRoutingConfiguration configuration) 
            : base(configuration)
        { }

        public override IOptionalRouteConstraintWrapper CreateOptionalRouteConstraint(object constraint)
        {
            return new OptionalRouteConstraintWrapper((IRouteConstraint)constraint);
        }
    }
}